using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Verse;
using RimWorld;

namespace RTFusebox
{
    public class IncidentWorker_RTSurgeProtected : IncidentWorker
    {
        private IEnumerable<Building_Battery> FullBatteries()
        {
            return
                from Building_Battery battery in Find.ListerBuildings.AllBuildingsColonistOfEType(EntityType.Building_Battery)
                where battery.GetComp<CompPowerBattery>().StoredEnergy > 50f
                select battery;
        }

        public override bool StorytellerCanUseNow()
        {
            return this.FullBatteries().Any<Building_Battery>();
        }

        public override bool TryExecute(IncidentParms parms)
        {
            List<Building_Battery> batteries = this.FullBatteries().ToList<Building_Battery>();
            if (batteries.Count<Building_Battery>() == 0)
            {
                return false;
            }

            PowerNet powerNet = batteries.RandomElement<Building_Battery>().PowerComp.PowerNet;
            List<CompPower> victimList = (
                from transmitter in powerNet.transmitters
                where transmitter.parent.def.eType == EntityType.Building_PowerConduit
                   || transmitter.parent.def.eType == EntityType.Wall
                select transmitter).ToList<CompPower>();
            if (victimList.Count == 0)
            {
                return false;
            }

            List<Building> surgeProtectors = (
                from transmitter in powerNet.transmitters
                where transmitter.parent.GetComp<CompRTSurgeProtector>() != null
                select transmitter.parent as Building).ToList<Building>();

            float energyTotal = 0f;
            foreach (CompPowerBattery battery in powerNet.batteryComps)
            {
                energyTotal += battery.StoredEnergy;
                battery.DrawPower(battery.StoredEnergy);
            }
            float energyTotalHistoric = energyTotal;
            foreach (Building surgeProtector in surgeProtectors)
            {
                energyTotal -= surgeProtector.GetComp<CompRTSurgeProtector>().MitigateDischarge(energyTotal);
                if (energyTotal <= 0)
                {
                    break;
                }
            }

            if (energyTotal == energyTotalHistoric)
            {
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
                if (victim.def.eType == EntityType.Building_PowerConduit)
                {
                    text = "AnElectricalConduit".Translate();
                }
                else if (victim.def.eType == EntityType.Wall)
                {
                    text = "AWallsPowerConduit".Translate();
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
                Find.History.AddGameEvent(stringBuilder.ToString(), GameEventType.BadNonUrgent, true, victim.Position, string.Empty);
                return true;
            }
            else if (energyTotal > 0)
            {
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
                if (victim.def.eType == EntityType.Building_PowerConduit)
                {
                    text = "AnElectricalConduit".Translate();
                }
                else if (victim.def.eType == EntityType.Wall)
                {
                    text = "AWallsPowerConduit".Translate();
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
                Find.History.AddGameEvent(stringBuilder.ToString(), GameEventType.BadNonUrgent, true, victim.Position, string.Empty);
                return true;
            }
            else
            {
                Thing victim = victimList.RandomElement<CompPower>().parent;
                victim.TakeDamage(new DamageInfo(DamageDefOf.Bomb, Rand.Range(0, (int)Math.Floor(0.1f * victim.MaxHealth)), null, null, null));
                string text = "a thing";
                if (victim.def.eType == EntityType.Building_PowerConduit)
                {
                    text = "AnElectricalConduit".Translate();
                }
                else if (victim.def.eType == EntityType.Wall)
                {
                    text = "AWallsPowerConduit".Translate();
                }
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("ShortCircuitProtected".Translate(new object[]{
				text,
				energyTotalHistoric.ToString("F0")}));
                Find.History.AddGameEvent(stringBuilder.ToString(), GameEventType.BadNonUrgent, true, victim.Position, string.Empty);
                return true;
            }
        }
    }
}
