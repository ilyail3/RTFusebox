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
        public void Kill()
        {
            this.ticksLeft = 0;
        }

        public int GetTicksToExpire()
        {
            return this.ticksLeft;
        }
    }
}
