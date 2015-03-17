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
    /// Swaps the dud solar flare with actual flare if no shield is present.
    /// </summary>
    public class MapComponent_RTSolarFlareSwapper : MapComponent
    {
        public static List<CompRTFlareProtector> shields = new List<CompRTFlareProtector>();
        private bool shieldsActivated = false;

        public override void MapComponentTick()
        {
            if (Find.TickManager.TicksGame % 10 != 0)
            {       // Run every 10th tick.
                return;
            }
            MapCondition_RTSolarFlare mapCondition = Find.MapConditionManager.GetActiveCondition<MapCondition_RTSolarFlare>();
            if (mapCondition != null)
            {
                if (shields.Count == 0 || !shields.Exists((CompRTFlareProtector shield) => shield.isActive))
                {
                    int ticksToExpire = mapCondition.GetTicksToExpire();
                    mapCondition.Kill();
                    Find.MapConditionManager.RegisterCondition(new MapCondition(MapConditionDefOf.SolarFlare, ticksToExpire));
                }
                else
                {
                    if (!shieldsActivated)
                    {
                        foreach (CompRTFlareProtector shield in shields)
                        {
                            CompPowerTrader powerComp = shield.parent.GetComp<CompPowerTrader>();
                            if (shield.isActive && powerComp != null)
                            {
                                powerComp.powerOutput = -shield.shieldingCost;
                            }
                        }
                        shieldsActivated = true;
                    }
                }
            }
            else
            {
                if (shieldsActivated)
                {
                    foreach (CompRTFlareProtector shield in shields)
                    {
                        CompPowerTrader powerComp = shield.parent.GetComp<CompPowerTrader>();
                        if (powerComp != null)
                        {
                            powerComp.powerOutput = -shield.idleCost;
                        }
                    }
                    shieldsActivated = false;
                }
            }
        }

        public override void ExposeData()
        {
            Scribe_Values.LookValue(ref shieldsActivated, "shieldsActivated", false);
        }
    }
}
