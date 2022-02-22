using Spellwright.Content.Spells.Base;
using System.Collections.Generic;

namespace Spellwright.Content.Spells
{
    internal static class SpellModifiersProcessor
    {
        private static readonly Dictionary<string, SpellModifier> modifierMap = new()
        {
            { "grand", SpellModifier.IsAoe },
            { "aoe", SpellModifier.IsAoe }
        };

        public static SpellModifier? GetModifier(string value)
        {
            if (!modifierMap.ContainsKey(value))
                return null;
            else
                return modifierMap[value];
        }
    }
}
