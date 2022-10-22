
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Izzimon.BloodDries
{
    public class BloodDries_MapComponent: MapComponent
    {

        public static bool DebuggingEnabled = false;


        public BloodDries_MapComponent(Map m): base(m)
        {
        }

        public override void FinalizeInit()
        {
            base.FinalizeInit();

            if (Find.CurrentMap != null) {
                var allThings = Find.CurrentMap.GetDirectlyHeldThings();
                var bloodToTidyUp = new List<Filth>();

                for (int i = 0; i < allThings.Count; i++)
                {
                    var thing = allThings[i];

                    if (thing.def.defName == "Filth_Blood")
                    {

                        if (!(thing is Blood thingAsBlood))
                        {
                            bloodToTidyUp.Add((Filth)thing);
                        }
                    }
                }

                if (bloodToTidyUp.Count > 0)
                {
                    Log.Message($"Found {bloodToTidyUp.Count} instances of blood that have the wrong class.  Replacing them with new copies.");
                }

                // now tidy them up
                foreach (var oldBlood in bloodToTidyUp)
                {
                    Logger.Debug($"Spawning new blood to replace {oldBlood}..");
                    var newBlood = (Filth)ThingMaker.MakeThing(oldBlood.def, null);
                    newBlood.AddSources(oldBlood.sources);
                    GenSpawn.Spawn(newBlood, oldBlood.Position, map, WipeMode.Vanish);

                    Logger.Debug($"Destroying {oldBlood}..");
                    oldBlood.Destroy(DestroyMode.Vanish);
                }
            }

        }

    }
}
