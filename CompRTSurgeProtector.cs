﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using UnityEngine;
using Verse;
using RimWorld;

namespace RTFusebox
{
    public class CompRTSurgeProtector : ThingComp
    {
        private CompProperties_RTFusebox compProps
        {
            get
            {
                return (CompProperties_RTFusebox)props;
            }
        }

        #region Overrides
        public override string CompInspectStringExtra()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("CompRTSurgeProtector_ProtectsAgainst".Translate(new object[] { (Math.Floor(compProps.surgeMitigation * (parent.Health - compProps.reserveHealthPercent * parent.MaxHealth) / parent.MaxHealth)).ToString("F0") }));
            if (compProps.reserveHealthPercent == 0.0f)
            {
                stringBuilder.Append(" " + "CompRTSurgeProtector_SingleUse".Translate());
            }
            return stringBuilder.ToString();
        }
        #endregion

        /// <summary>
        /// Tries to mitigate amount of charge, making parent take damage in the process.
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="reserveHealthPercent"></param>
        /// <returns>Mitigated amount.</returns>
        public float MitigateDischarge(float amount)
        {
            float amountMitigated = Mathf.Clamp(Math.Min((float)Math.Floor(compProps.surgeMitigation * (parent.Health - compProps.reserveHealthPercent * parent.MaxHealth) / parent.MaxHealth), amount), 0, amount);
            parent.TakeDamage(new DamageInfo(DamageDefOf.Bomb, (int)(parent.MaxHealth * amountMitigated / compProps.surgeMitigation), null, null, null));
            return amountMitigated;
        }
    }
}