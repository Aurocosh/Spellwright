using Spellwright.Content.Spells.Base.Modifiers;
using System.Collections.Generic;

namespace Spellwright.Content.Spells
{
    internal static class SpellModifiersProcessor
    {
        private static readonly Dictionary<string, SpellModifier> modifierMap = new()
        {
            { "unlock", SpellModifier.IsUnlock },
            { "area", SpellModifier.IsAoe },
            { "mass", SpellModifier.IsAoe },
            { "eternal", SpellModifier.IsEternal },
            { "dispel", SpellModifier.IsDispel },
            { "selfless", SpellModifier.IsSelfless },
            { "twofold", SpellModifier.IsTwofold },
            { "fivefold", SpellModifier.IsFivefold },
            { "tenfold", SpellModifier.IsTenfold },
            { "fiftyfold", SpellModifier.IsFiftyfold }
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
