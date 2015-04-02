using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Verse;
using RimWorld;

namespace RTFusebox
{
    public static class ResearchModsSpecial
    {
        public static void SurgeProtectedInjector()
        {
            IncidentDef incidentDef = DefDatabase<IncidentDef>.GetNamed("ShortCircuit");
            incidentDef.workerClass = typeof(IncidentWorker_RTSurgeProtected);
        }

        public static void FlareProtectedInjector()
        {
            IncidentDef incidentDef = DefDatabase<IncidentDef>.GetNamed("SolarFlare");
            incidentDef.workerClass = typeof(IncidentWorker_RTFlareProtected);
            Log.Message(incidentDef.workerClass.ToString());
            if (Find.Map != null && Find.Map.components != null)
            {
                if (Find.Map.components.FindAll(x => x.GetType().ToString() == "MapComponent_RTFusebox").Count != 0)
                {
                    Log.Message("MapComponentInjector: map already has MapComponent_RTFusebox.");
                }
                else
                {
                    Log.Message("MapComponentInjector: adding MapComponent_RTFusebox...");
                    Find.Map.components.Add(new MapComponent_RTFusebox());
                    Log.Message("MapComponentInjector: success!");
                }
            }
        }
    }
}
