using System.Collections.Generic;

namespace Spellwright.Util
{
    internal static class UtilList
    {
        public static IEnumerable<T> Singleton<T>(T element)
        {
            yield return element;
        }
    }
}
