using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Verse;
using RimWorld;

namespace RTFusebox       // Replace with yours.
{       // This code is mostly borrowed from Pawn State Icons mod by Dan Sadler, which has open source and no license I could find, so...
    class MapComponentInjectorBehavior : MonoBehaviour
    {
        public static readonly string mapComponentName = "RTFusebox.MapComponent_RTFusebox";       // Ditto.
        private RTFusebox.MapComponent_RTFusebox mapComponent = new RTFusebox.MapComponent_RTFusebox();       // Ditto.

        #region Irrelevant
        protected bool reinjectNeeded = false;
        protected float reinjectTime = 0;

        public void OnLevelWasLoaded(int level)
        {
            reinjectNeeded = true;
            if (level >= 0)
            {
                reinjectTime = 1;
            }
            else
            {
                reinjectTime = 0;
            }
        }

        public void FixedUpdate()
        {
            if (reinjectNeeded)
            {
                reinjectTime -= Time.fixedDeltaTime;
                if (reinjectTime <= 0)
                {
                    reinjectNeeded = false;
                    reinjectTime = 0;
                    if (Find.Map != null && Find.Map.components != null)
                    {
                        if (Find.Map.components.FindAll(x => x.GetType().ToString() == mapComponentName).Count != 0)
                        {
                            Log.Message("MapComponentInjector: map already has " + mapComponentName + ".");
                            //Destroy(gameObject);
                        }
                        else
                        {
                            Log.Message("MapComponentInjector: adding " + mapComponentName + "...");
                            Find.Map.components.Add(mapComponent);
                            Log.Message("MapComponentInjector: success!");
                            //Destroy(gameObject);
                        }
#endregion
                        #region Relevant
                        // Replace incident workers with custom ones.
                        IncidentDef incidentDef = DefDatabase<IncidentDef>.GetNamed("ShortCircuit");
                        incidentDef.workerClass = typeof(IncidentWorker_RTSurgeProtected);
                        incidentDef = DefDatabase<IncidentDef>.GetNamed("SolarFlare");
                        incidentDef.workerClass = typeof(IncidentWorker_RTFlareProtected);
                        #endregion
                        #region Irrelevant
                    }
                }
            }
        }

        public void Start()
        {
            OnLevelWasLoaded(-1);
        }
    }

    class MapComponentInjector : ITab
    {
        protected UnityEngine.GameObject initializer;

        public MapComponentInjector()
        {
            Log.Message("MapComponentInjector: initializing for " + MapComponentInjectorBehavior.mapComponentName);
            initializer = new UnityEngine.GameObject("MapComponentInjector");
            initializer.AddComponent<MapComponentInjectorBehavior>();
            UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object)initializer);
        }

        protected override void FillTab()
        {

        }
    }
}
        #endregion