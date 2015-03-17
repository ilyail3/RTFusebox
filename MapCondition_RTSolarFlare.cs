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
    /// Dummy replacement MapCondition for solar flares.
    /// </summary>
    public class MapCondition_RTSolarFlare : MapCondition
    {
        public MapCondition_RTSolarFlare()
        {

        }

        public MapCondition_RTSolarFlare(int duration)
            : base(DefDatabase<MapConditionDef>.GetNamed("RTFusebox_SolarFlare"), duration)
        {

        }

        public void Kill()
        {
            this.ticksToExpire = 0;
        }

        public int GetTicksToExpire()
        {
            return this.ticksToExpire;
        }
    }
}
