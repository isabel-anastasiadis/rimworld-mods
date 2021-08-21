using RimWorld;
using System;
using UnityEngine;
using Verse;

namespace Izzimon.BloodDries
{
    public class Blood : Filth
    {
        // to add to the def to configure later
        private static int FreshBloodR = 131;
        private static int FreshBloodG = 34;
        private static int FreshBloodB = 34;

        private static int DriedBloodR = 35;
        private static int DriedBloodG = 14;
        private static int DriedBloodB = 14;
                
        private static int DefaultAlpha = 180;
        private static int MinimumAlpha = 15;

        private static int StandardTemperature = 20;
        private static int DaysUntilFullyDryAtStandardTemperature = 3;

        private float percentageDried = 0;
        private float percentageEroded = 0;

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);

            if (!FilthMaker.TerrainAcceptsFilth(base.Map.terrainGrid.TerrainAt(base.Position), def))
            {
                // it will have been destroyed already, so do nothing.
                return;
            }
        }


        public override Color DrawColor
        {
            get
            {
                var red = GetWeightedAverage(FreshBloodR, DriedBloodR, percentageDried) / 255;
                var green = GetWeightedAverage(FreshBloodG, DriedBloodG, percentageDried) / 255;
                var blue = GetWeightedAverage(FreshBloodB, DriedBloodB, percentageDried) / 255;
                var alpha = GetWeightedAverage(DefaultAlpha, MinimumAlpha, percentageEroded) / 255;

                var color = new Color(red, green, blue, alpha);

                return color;
            }
            set
            {
                Log.Error(string.Concat(new object[]
                {
                    "Cannot set instance color on non-ThingWithComps ",
                    this.LabelCap,
                    " at ",
                    this.Position,
                    "."
                }));
            }
        }

        public override void TickLong()
        {
            bool changed = false;
            
            changed |= DryMore();

            changed |= ErodeMore();

            if (percentageEroded == 1f)
            {
                this.Destroy(DestroyMode.Vanish);
            }
            else if (changed)
            {
                this.Notify_ColorChanged();
            }

        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<float>(ref this.percentageDried, "percentageDried", 0f, false);
        }

        private bool DryMore() 
        {
            if (this.percentageDried == 1f) {
                return false;
            }

            float temperatureModifier = 1f;
            if (this.AmbientTemperature <= 0)
            {
                return false;  // it is frozen, don't dry any more
            }
            else if (this.AmbientTemperature > StandardTemperature) {
                temperatureModifier += (this.AmbientTemperature - StandardTemperature)*2 / StandardTemperature;  // add a bonus if the temperature is hot
            }
                
            var percentageMoreToDry = 2000f / (60000 * DaysUntilFullyDryAtStandardTemperature) * temperatureModifier;  // long tick = 2000 ticks.  one day in game time = 60000 ticks.

            this.percentageDried += percentageMoreToDry;
            this.percentageDried = Math.Min(this.percentageDried, 1f);

            return true;
        }
        
        private bool ErodeMore() 
        {

            var percentageMoreToErode = 2000f / this.DisappearAfterTicks;

            this.percentageEroded += percentageMoreToErode;
            this.percentageEroded = Math.Min(this.percentageEroded, 1f);

            return true;
        }
        
        private float GetWeightedAverage(float from, float to, float transitionPercentage)
        {
            var difference = (to - from) * transitionPercentage;
            return from + difference;
        }

    }
}
