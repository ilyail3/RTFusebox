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
            int ticksToExpire = Rand.Range((int)((1 / GenTime.TicksToDays(1)) / 4), (int)(1 / GenTime.TicksToDays(1)));     // Range between 1/4th of a day and a full day.
            Find.MapConditionManager.RegisterCondition(new MapCondition_RTSolarFlare(ticksToExpire));
            Find.LetterStack.ReceiveLetter("LetterLabelSolarFlare".Translate(), "LetterSolarFlare".Translate(), LetterType.BadNonUrgent, null);
            return true;
        }
    }
}
