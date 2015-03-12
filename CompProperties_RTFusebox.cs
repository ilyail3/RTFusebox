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
    /// Properties class for components form the RTPreparedness
    /// </summary>
    public class CompProperties_RTFusebox : CompProperties
    {
        public float surgeMitigation = 1000;
        public float reserveHealthPercent = 0.05f;

        public CompProperties_RTFusebox()
        {
            this.compClass = typeof(CompProperties_RTFusebox);
        }
    }
}
