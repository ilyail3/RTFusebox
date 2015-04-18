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
        public float heatingPerTick = 0.0f;
        public string rotatorPath = "RT_Buildings/RTFlareShieldTop";
        public float rotatorSpeedIdle = 0.5f;
        public float rotatorSpeedWorking = 10.0f;

        public CompProperties_RTFusebox()
        {
            this.compClass = typeof(CompProperties_RTFusebox);
        }
    }
}
