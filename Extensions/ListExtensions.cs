using System.Collections.Generic;
using Terraria;

namespace Spellwright.Extensions
{
    internal static class ListExtensions
    {
        public static T GetRandom<T>(this IList<T> list)
        {
            int index = Main.rand.Next(0, list.Count);
            return list[index];
        }
        public static void Shuffle<T>(this IList<T> list)
        {
            int i = list.Count;
            while (i > 1)
            {
                int j = Main.rand.Next(i--);
                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }
    }
}
