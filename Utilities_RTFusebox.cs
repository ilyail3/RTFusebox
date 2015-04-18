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
        public static PowerState CheckPower(this Thing parent)
        {
            CompPowerTrader powerComp = parent.TryGetComp<CompPowerTrader>();
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

        /// <summary>
        /// NRE-safe way to toggle glow.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="state"></param>
        public static void GlowOn(this Thing parent, bool state)
        {
            CompGlower glowComp = parent.TryGetComp<CompGlower>();
            if (glowComp != null)
            {
                glowComp.Lit = state;
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
