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


        private int gameTickConstructedIn; 


        public Blood() {
            this.gameTickConstructedIn = Find.TickManager.TicksGame;      
        }

        private float PercentageDried { 
            get {
                var ageInTicks = Find.TickManager.TicksGame - this.gameTickConstructedIn;

                var ageInDays = ageInTicks / 60000f;

                return Math.Min(ageInDays/ DaysUntilFullyDry, 1f);
            } 
        }

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


        private float GetCurrentValue(float from, float to) {

            var difference = (to - from) * PercentageDried;
            return from + difference;
        }


        public override Color DrawColor
        {
            get
            {
                var red = GetCurrentValue(FreshBloodColor.r, DriedBloodColor.r);
                var green = GetCurrentValue(FreshBloodColor.g, DriedBloodColor.g);
                var blue = GetCurrentValue(FreshBloodColor.b, DriedBloodColor.b);

                return new Color(red, green, blue, 180);
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



    }
}
