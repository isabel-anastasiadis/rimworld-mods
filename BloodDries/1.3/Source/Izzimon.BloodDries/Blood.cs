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

        private static int DriedBloodR = 55;
        private static int DriedBloodG = 24;
        private static int DriedBloodB = 24;
                
        private static int DefaultAlpha = 180;

        private static int StandardTemperature = 20;
        private static int DaysUntilFullyDryAtStandardTemperature = 1;
        private static int DaysUntilFullyEroded = 20;


        private float percentageDried = 0;
        private float percentageEroded = 0;

        private TerrainDef terrainType;

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);

            if (!FilthMaker.TerrainAcceptsFilth(base.Map.terrainGrid.TerrainAt(base.Position), def))
            {
                // it will have been destroyed already, so do nothing.
                return;
            }

            // save a reference to the terrain type it is on
            this.terrainType = base.Map.terrainGrid.TerrainAt(base.Position);

            // raise a message to ensure we've hooked into it correctly
            //MoteMaker.ThrowText(base.Position.ToVector3(), base.Map, "Blood spawned", 3f);
        }


        public override Color DrawColor
        {
            get
            {
                var red = GetWeightedAverage(FreshBloodR, DriedBloodR, percentageDried) / 255;
                var green = GetWeightedAverage(FreshBloodG, DriedBloodG, percentageDried) / 255;
                var blue = GetWeightedAverage(FreshBloodB, DriedBloodB, percentageDried) / 255;
                var alpha = GetWeightedAverage(DefaultAlpha, 0, percentageEroded) / 255;

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
            DryMore();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<float>(ref this.percentageDried, "percentageDried", 0f, false);
        }

        private void DryMore() {
            if (this.percentageDried == 1f) {
                return;
            }

            float temperatureModifier = 1f;
            if (this.AmbientTemperature <= 0)
            {
                return;  // it is frozen, don't dry any more
            }
            else if (this.AmbientTemperature > StandardTemperature) {
                temperatureModifier += (this.AmbientTemperature - StandardTemperature) / StandardTemperature;  // add a bonus if the temperature is hot
            }
                
            var percentageMoreToDry = 2000f / (60000 * DaysUntilFullyDryAtStandardTemperature) * temperatureModifier;  // long tick = 2000 ticks.  one day in game time = 60000 ticks.

            this.percentageDried += percentageMoreToDry;
            this.percentageDried = Math.Min(this.percentageDried, 1f);

            this.Notify_ColorChanged(); 
        }

        private float GetWeightedAverage(float from, float to, float transitionPercentage)
        {
            var difference = (to - from) * transitionPercentage;
            return from + difference;
        }

    }
}
