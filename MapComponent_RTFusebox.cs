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
    /// Swaps the dud solar flare with actual flare if no shield is present, and makes side effects happen.
    /// </summary>
    public class MapComponent_RTFusebox : MapComponent
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
                                powerComp.powerOutputInt = -shield.shieldingCost;
                            }
                        }
                        shieldsActivated = true;
                    }
                    else
                    {
                        foreach (CompRTFlareProtector shield in shields)
                        {
                            Room room = shield.parent.GetRoom();
                            if (room != null
                                && !room.UsesOutdoorTemperature)
                            {
                                room.Temperature += shield.heatingPerTick;
                            }
                        }
                        List<Building_CommsConsole> comms = Find.ListerBuildings.AllBuildingsColonistOfClass<Building_CommsConsole>().ToList();
                        foreach (Building_CommsConsole comm in comms)
                        {
                            CompPowerTrader component = comm.TryGetComp<CompPowerTrader>();
                            if (component != null)
                            {
                                component.PowerOn = false;
                            }
                        }
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
                            powerComp.powerOutputInt = -shield.idleCost;
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
