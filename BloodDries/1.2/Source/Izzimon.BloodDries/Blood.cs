using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Izzimon.BloodDries
{
    public class Blood : Filth
    {
        // to add to the def to configure later
        private Color freshBloodColor = new Color(131, 34, 34, 180);
        private Color driedBloodColor = new Color(77, 54, 54, 180);
        private static int daysUntilFullyDry = 3;
        private float ticksUntilFullyDry = daysUntilFullyDry * 60000f;


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

        // this doesn't seem to be working for some reason
        public override Graphic Graphic {

            get {

                var graphic = DefaultGraphic;

                graphic.color = new Color(driedBloodColor.r, driedBloodColor.g, driedBloodColor.b, 180);

                return graphic;
            }
        }

    }
}
