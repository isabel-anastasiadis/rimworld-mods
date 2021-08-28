
using System.Linq;
using Verse;

namespace Izzimon.BloodDries
{
    public class BloodDriesGameComponent: GameComponent
    {

        public BloodDriesGameComponent(Game _) 
        {
        }

        public override void LoadedGame()
        {
            base.LoadedGame();

            if (!ScribeMetaHeaderUtility.loadedModNamesList.Any(modName => modName == "Blood Dries")) {

                Log.Message("File was not saved with Blood Dries mod active");


            }
        }

    }
}
