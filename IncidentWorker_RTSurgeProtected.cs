using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Verse;
using RimWorld;

namespace RTFusebox
{
    /// <summary>
    /// Replacement IncidentWorker that takes fuses into account.
    /// </summary>
    public class IncidentWorker_RTSurgeProtected : IncidentWorker
    {
        private IEnumerable<Building_Battery> FullBatteries()
        {
            return
                from Building battery in Find.ListerBuildings.allBuildingsColonist
                where battery.TryGetComp<CompPowerBattery>() != null
                && battery.TryGetComp<CompPowerBattery>().StoredEnergy > 50f
                select battery as Building_Battery;
        }

        public override bool StorytellerCanUseNow()
        {
            return this.FullBatteries().Any<Building_Battery>();
        }

        public override bool TryExecute(IncidentParms parms)
        {
            List<Building_Battery> batteries = this.FullBatteries().ToList<Building_Battery>();
            if (batteries.Count<Building_Battery>() == 0)
            {       // Check for existing batteries.
                return false;
            }

            PowerNet powerNet = batteries.RandomElement<Building_Battery>().PowerComp.PowerNet;     // Choose a powernet with batteries.
            List<CompPower> victimList = (
                from transmitter in powerNet.transmitters
                where transmitter.parent.def == ThingDefOf.PowerConduit
                select transmitter).ToList<CompPower>();
            if (victimList.Count == 0)
            {       // Form a list of things that can get shorted on the chosen powerned.
                return false;
            }

            List<Building> surgeProtectors = (
                from transmitter in powerNet.transmitters
                where transmitter.parent.GetComp<CompRTSurgeProtector>() != null
                select transmitter.parent as Building).ToList<Building>();
                    // Form a list of fuses in the chosen powernet.

            float energyTotal = 0f;
            foreach (CompPowerBattery battery in powerNet.batteryComps)
            {       // Discharge batteries.
                energyTotal += battery.StoredEnergy;
                battery.DrawPower(battery.StoredEnergy);
            }
            float energyTotalHistoric = energyTotal;
            foreach (Building surgeProtector in surgeProtectors)
            {       // Try to mitigate the discharge with fuses.
                energyTotal -= surgeProtector.GetComp<CompRTSurgeProtector>().MitigateDischarge(energyTotal);
                if (energyTotal <= 0)
                {
                    break;
                }
            }

            if (energyTotal == energyTotalHistoric)
            {       // No mitigation, vanilla behavior.
                float explosionRadius = Mathf.Sqrt(energyTotal) * 0.05f;
                if (explosionRadius > 14.9f)
                {
                    explosionRadius = 14.9f;
                }
                Thing victim = victimList.RandomElement<CompPower>().parent;
                GenExplosion.DoExplosion(victim.Position, explosionRadius, DamageDefOf.Flame, null, null, null);
                if (explosionRadius > 3.5f)
                {
                    GenExplosion.DoExplosion(victim.Position, explosionRadius * 0.3f, DamageDefOf.Bomb, null, null, null);
                }
                if (!victim.Destroyed)
                {
                    victim.TakeDamage(new DamageInfo(DamageDefOf.Bomb, 200, null, null, null));
                }
                string text = "a thing";
                if (victim.def == ThingDefOf.PowerConduit)
                {
                    text = "AnElectricalConduit".Translate();
                }
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("ShortCircuit".Translate(new object[]{
				text,
				energyTotalHistoric.ToString("F0"),
                (energyTotalHistoric - energyTotal).ToString("F0")}));
                if (explosionRadius > 5f)
                {
                    stringBuilder.AppendLine();
                    stringBuilder.AppendLine();
                    stringBuilder.Append("ShortCircuitWasLarge".Translate());
                }
                if (explosionRadius > 8f)
                {
                    stringBuilder.AppendLine();
                    stringBuilder.AppendLine();
                    stringBuilder.Append("ShortCircuitWasHuge".Translate());
                }
                Find.LetterStack.ReceiveLetter("LetterLabelShortCircuit".Translate(), stringBuilder.ToString(), LetterType.BadNonUrgent, victim.Position, null);
                return true;
            }
            else if (energyTotal > 0)
            {       // Partial mitigation.
                float explosionRadius = Mathf.Sqrt(energyTotal) * 0.05f;
                if (explosionRadius > 14.9f)
                {
                    explosionRadius = 14.9f;
                }
                Thing victim = victimList.RandomElement<CompPower>().parent;
                GenExplosion.DoExplosion(victim.Position, explosionRadius, DamageDefOf.Flame, null, null, null);
                if (explosionRadius > 3.5f)
                {
                    GenExplosion.DoExplosion(victim.Position, explosionRadius * 0.3f, DamageDefOf.Bomb, null, null, null);
                }
                if (!victim.Destroyed)
                {
                    victim.TakeDamage(new DamageInfo(DamageDefOf.Bomb, 200, null, null, null));
                }
                string text = "a thing";
                if (victim.def == ThingDefOf.PowerConduit)
                {
                    text = "AnElectricalConduit".Translate();
                }
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("ShortCircuitPartial".Translate(new object[]{
				text,
				energyTotalHistoric.ToString("F0"),
                (energyTotalHistoric - energyTotal).ToString("F0")}));
                if (explosionRadius > 5f)
                {
                    stringBuilder.AppendLine();
                    stringBuilder.AppendLine();
                    stringBuilder.Append("ShortCircuitWasLarge".Translate());
                }
                if (explosionRadius > 8f)
                {
                    stringBuilder.AppendLine();
                    stringBuilder.AppendLine();
                    stringBuilder.Append("ShortCircuitWasHuge".Translate());
                }
                Find.LetterStack.ReceiveLetter("LetterLabelShortCircuit".Translate(), stringBuilder.ToString(), LetterType.BadNonUrgent, victim.Position, null);
                return true;
            }
            else
            {       // Full mitigation.
                Thing victim = victimList.RandomElement<CompPower>().parent;
                victim.TakeDamage(new DamageInfo(DamageDefOf.Bomb, Rand.Range(0, (int)Math.Floor(0.1f * victim.MaxHitPoints)), null, null, null));
                string text = "a thing";
                if (victim.def == ThingDefOf.PowerConduit)
                {
                    text = "AnElectricalConduit".Translate();
                }
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("ShortCircuitProtected".Translate(new object[]{
				text,
				energyTotalHistoric.ToString("F0")}));
                Find.LetterStack.ReceiveLetter("LetterLabelShortCircuit".Translate(), stringBuilder.ToString(), LetterType.BadNonUrgent, victim.Position, null);
                return true;
            }
        }
    }
}
