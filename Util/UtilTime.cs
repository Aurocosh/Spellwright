using System;
using System.Collections.Generic;

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


        public static string TicksToString(int ticks)
        {
            int seconds = ticks / TicksInSecond;

            int hours = (int)Math.Floor(seconds / 3600f);
            int secondsInHours = hours * 3600;
            seconds -= secondsInHours;

            int minutes = (int)Math.Floor(seconds / 60f);
            int secondsInMinutes = minutes * 60;
            seconds -= secondsInMinutes;

            var timeParts = new List<string>();
            if (hours > 0)
                timeParts.Add(hours.ToString());

            timeParts.Add(minutes.ToString());
            timeParts.Add(seconds.ToString());

            return string.Join(":", timeParts);
        }
    }
}
