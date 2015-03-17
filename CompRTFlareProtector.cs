using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Verse;
using RimWorld;

namespace RTFusebox
{
    public class CompRTFlareProtector : ThingComp
    {
        private CompProperties_RTFusebox compProps
        {
            get
            {
                return (CompProperties_RTFusebox)props;
            }
        }
        public bool causeHeadaches
        {
            get
            {
                return this.compProps.causeHeadaches;
            }
        }
        public float idleCost
        {
            get
            {
                return this.compProps.idleCost;
            }
        }
        public float shieldingCost
        {
            get
            {
                return this.compProps.shieldingCost;
            }
        }
        public bool isActive
        {
            get
            {
                return (parent.CheckPower() != PowerState.Off);
            }
        }

        #region Overrides
        public override void PostSpawnSetup()
        {
            MapComponent_RTSolarFlareSwapper.shields.Add(this);
        }

        public override void PostDeSpawn()
        {
            MapComponent_RTSolarFlareSwapper.shields.Remove(this);
        }

        public override string CompInspectStringExtra()
        {
            return "CompRTFlareShield_FlareProtection".Translate();
        }
        #endregion
    }
}
