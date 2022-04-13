using Spellwright.Content.Spells.Base;
using System.Collections.Generic;

namespace Spellwright.Content.Spells
{
    internal static class SpellModifiersProcessor
    {
        private static readonly Dictionary<string, SpellModifier> modifierMap = new()
        {
            { "area", SpellModifier.IsAoe },
            { "mass", SpellModifier.IsAoe },
            { "eternal", SpellModifier.IsEternal },
            { "dispel", SpellModifier.IsDispel }
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
