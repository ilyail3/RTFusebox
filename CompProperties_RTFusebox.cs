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
    /// Properties class for components from RTFusebox.
    /// </summary>
    public class CompProperties_RTFusebox : CompProperties
    {
        public float surgeMitigation = 1000;
        public float reserveHealthPercent = 0.05f;

        public float shieldingCost = 0;
        public float idleCost = 0;
        public bool causeHeadaches = false;

        public CompProperties_RTFusebox()
        {
            this.compClass = typeof(CompProperties_RTFusebox);
        }
    }
}
