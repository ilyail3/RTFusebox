using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Verse;
using RimWorld;

namespace RTFusebox
{
    public class IncidentWorker_RTFlareProtected : IncidentWorker
    {
        public override bool StorytellerCanUseNow()
        {
            MapConditionDef mapConditionDef = DefDatabase<MapConditionDef>.GetNamed("RTFusebox_SolarFlare");
            return !(Find.MapConditionManager.ConditionIsActive(mapConditionDef)
                || Find.MapConditionManager.ConditionIsActive(MapConditionDefOf.SolarFlare));
        }

        public override bool TryExecute(IncidentParms parms)
        {
            MapConditionDef mapConditionDef = DefDatabase<MapConditionDef>.GetNamed("RTFusebox_SolarFlare");
            if (Find.MapConditionManager.ConditionIsActive(MapConditionDefOf.SolarFlare)
                || Find.MapConditionManager.ConditionIsActive(mapConditionDef))
            {
                return false;
            }
            //int ticksToExpire = Rand.Range(8000, 24000);
            int ticksToExpire = Rand.Range(1000, 2000);
            Find.MapConditionManager.RegisterCondition(new MapCondition_RTSolarFlare(ticksToExpire));
            Find.History.AddGameEvent("FlareProtected".Translate(), GameEventType.BadNonUrgent, true, string.Empty);
            return true;
        }
    }
}
