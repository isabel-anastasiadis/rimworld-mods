using RimWorld;
using System;
using UnityEngine;
using Verse;

namespace Izzimon.BloodDries
{
    public class Blood : Filth
    {
        // to add to the def to configure later
        private static Color FreshBloodColor = new Color(131, 34, 34, 180);
        private static Color DriedBloodColor = new Color(77, 54, 54, 180);
        private static int DaysUntilFullyDry = 1;

        private float percentageDried = 0;

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);

            if (!FilthMaker.TerrainAcceptsFilth(base.Map.terrainGrid.TerrainAt(base.Position), def))
            {
                // it will have been destroyed already, so do nothing.
                return;
            }

            // save a reference to the terrain type it is on

            // raise a message to ensure we've hooked into it correctly
            MoteMaker.ThrowText(base.Position.ToVector3(), base.Map, "Blood spawned", 3f);
        }


        public override Color DrawColor
        {
            get
            {
                var red = GetCurrentValue(FreshBloodColor.r, DriedBloodColor.r)/255;
                var green = GetCurrentValue(FreshBloodColor.g, DriedBloodColor.g)/255;
                var blue = GetCurrentValue(FreshBloodColor.b, DriedBloodColor.b)/255;
                var alpha = 180 / 255f;

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

            var percentageToDryPerHour = 2000f / 60000 * DaysUntilFullyDry;  // long tick = 2000 ticks.  one day in game time = 60000 ticks.

            this.percentageDried += percentageToDryPerHour;
            this.percentageDried = Math.Min(this.percentageDried, 1f);

            this.Notify_ColorChanged(); // do we need this?
        }



        private float GetCurrentValue(float from, float to)
        {
            var difference = (to - from) * this.percentageDried;
            return from + difference;
        }


    }
}
