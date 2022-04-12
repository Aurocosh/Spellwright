using System;
using System.Reflection;
using Terraria.ModLoader;

namespace Spellwright.Util
{
    internal static class UtilLang
    {
        private delegate ModTranslation GetOrCreateDelegate(string key, bool defaultEmpty);
        private static readonly GetOrCreateDelegate methodDelegate;

        static UtilLang()
        {
            MethodInfo methodInfo = typeof(LocalizationLoader).GetMethod("GetOrCreateTranslation", BindingFlags.NonPublic | BindingFlags.Static, new Type[] { typeof(string), typeof(bool) });
            methodDelegate = (GetOrCreateDelegate)Delegate.CreateDelegate(typeof(GetOrCreateDelegate), null, methodInfo, true);
        }

        public static ModTranslation GetOrCreateTranslation(string key, bool defaultEmpty = false)
        {
            return methodDelegate(key, defaultEmpty);
        }
    }
}
