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
    }
}
