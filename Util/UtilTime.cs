using System;
using System.Collections.Generic;

namespace Spellwright.Util
{
    internal static class UtilTime
    {
        private static readonly int TicksInSecond = 60;

        public static int SecondsToTicks(float seconds)
        {
            return (int)(seconds * TicksInSecond);
        }

        public static int MinutesToTicks(float minutes, float seconds = 0)
        {
            return (int)(((minutes * 60) + seconds) * TicksInSecond);
        }

        public static int HoursToTicks(float hours, float minutes = 0, float seconds = 0)
        {
            return (int)(((hours * 3600) * (minutes * 60) + seconds) * TicksInSecond);
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
            {
                timeParts.Add(hours.ToString() + "h");
            }

            if (minutes > 0)
            {
                timeParts.Add(minutes.ToString() + "m");
            }

            if (seconds > 0 || timeParts.Count == 0)
            {
                timeParts.Add(seconds.ToString() + "s");
            }

            return string.Join(" ", timeParts);
        }
    }
}
