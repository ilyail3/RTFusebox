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
    /// Static class with utility extension functions.
    /// </summary>
    public static class Utilities_RTFusebox
    {
        /// <summary>
        /// NRE-safe way to check power.
        /// </summary>
        /// <param name="parent"></param>
        /// <returns>PowerState of parent.</returns>
        public static PowerState CheckPower(this ThingWithComponents parent)
        {
            CompPowerTrader powerComp = parent.GetComp<CompPowerTrader>();
            if (powerComp != null)
            {
                if (powerComp.PowerOn)
                {
                    return PowerState.On;
                }
                else
                {
                    return PowerState.Off;
                }
            }
            else
            {
                return PowerState.None;
            }
        }
    }

    public enum PowerState
    {
        None,
        Off,
        On
    }
}
