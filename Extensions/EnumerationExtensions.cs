using System;
using System.Collections.Generic;

namespace Spellwright.Extensions
{
    internal static class EnumerationExtensions
    {
        public static T Add<T>(this Enum type, T value)
        {
            int valueA = (int)(object)type;
            int valueB = (int)(object)value;
            return (T)(object)(valueA | valueB);
        }

        public static T Remove<T>(this Enum type, T value)
        {
            int valueA = (int)(object)type;
            int valueB = (int)(object)value;
            return (T)(object)(valueA & ~valueB);
        }
        public static T Toggle<T>(this Enum type, T value)
        {
            int valueA = (int)(object)type;
            int valueB = (int)(object)value;
            return (T)(object)(valueA ^ valueB);
        }
        public static bool HasAll<T>(this Enum type, T value)
        {
            int valueA = (int)(object)type;
            int valueB = (int)(object)value;
            return (valueA & valueB) == valueB;
        }
        public static bool HasAny<T>(this Enum type, T value)
        {
            int valueA = (int)(object)type;
            int valueB = (int)(object)value;
            return (valueA & valueB) != 0;
        }
        public static bool MissesAny<T>(this Enum type, T value)
        {
            int valueA = (int)(object)type;
            int valueB = (int)(object)value;
            return (valueB & ~valueA) != 0;
        }
        public static IEnumerable<T> SplitValues<T>(this Enum type)
        {
            int valueA = (int)(object)type;
            foreach (T enumValue in Enum.GetValues(typeof(T)))
            {
                int valueB = (int)(object)enumValue;
                if (valueB != 0 && (valueA & valueB) == valueB)
                    yield return enumValue;
            }
        }
    }
}
