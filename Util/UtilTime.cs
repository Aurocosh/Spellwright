namespace Spellwright.Util
{
    internal static class UtilTime
    {
        private static readonly int TicksInSecond = 60;

        public static int SecondsToTicks(int seconds)
        {
            return seconds * TicksInSecond;
        }

        public static int MinutesToTicks(int minutes, int seconds = 0)
        {
            return ((minutes * 60) + seconds) * TicksInSecond;
        }

        public static int HoursToTicks(int hours, int minutes = 0, int seconds = 0)
        {
            return ((hours * 3600) * (minutes * 60) + seconds) * TicksInSecond;
        }
    }
}
