
using Verse;

namespace Izzimon.BloodDries
{
    public static class Logger
    {
        public static bool DebugEnabled = true;

        public static void Debug(string message) {
            if (DebugEnabled) {
                Log.Message(message);
            }
        }

    }
}
