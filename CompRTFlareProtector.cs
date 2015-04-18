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
        public float heatingPerTick
        {
            get
            {
                return compProps.heatingPerTick;
            }
        }
        public bool isActive
        {
            get
            {
                return (parent.CheckPower() != PowerState.Off);
            }
        }
        private Material rotator;
        private float angle = (float)Rand.Range(0, 360);

        #region Overrides
        public override void PostSpawnSetup()
        {
            MapComponent_RTFusebox.shields.Add(this);
            rotator = MaterialPool.MatFrom(compProps.rotatorPath, ShaderDatabase.Cutout);
        }

        public override void PostDeSpawn()
        {
            MapComponent_RTFusebox.shields.Remove(this);
        }

        public override string CompInspectStringExtra()
        {
            return "CompRTFlareShield_FlareProtection".Translate();
        }

        public override void CompTick()
        {
            FlareProtectorTick(1);
        }

        public override void CompTickRare()
        {
            FlareProtectorTick(250);
        }

        public override void PostDraw()
        {       // Thanks Skullywag!
            Vector3 vector = new Vector3(1f, 1f, 1f);
            vector.y = Altitudes.AltitudeFor(AltitudeLayer.VisEffects);
            Matrix4x4 matrix = default(Matrix4x4);
            matrix.SetTRS(this.parent.DrawPos + Altitudes.AltIncVect, Quaternion.AngleAxis(angle, Vector3.up), vector);
            Graphics.DrawMesh(MeshPool.plane10, matrix, rotator, 0);
        }
        #endregion

        private void FlareProtectorTick(int tickAmount)
        {
            if (isActive)
            {
                if (Find.MapConditionManager.GetActiveCondition<MapCondition_RTSolarFlare>() != null)
                {
                    parent.GlowOn(true);
                    angle += compProps.rotatorSpeedWorking * tickAmount;
                }
                else
                {
                    parent.GlowOn(false);
                    angle += compProps.rotatorSpeedIdle * tickAmount;
                }
            }
        }
    }
}
