using Spellwright.Content.Spells.Base.Modifiers;
using System;
using System.Collections.Generic;

namespace Spellwright.Content.Spells
{
    internal static class SpellModifiersProcessor
    {
        private static readonly Dictionary<string, SpellModifier> modifierMap = new()
        {
            { "unlock", SpellModifier.Unlock },
            { "area", SpellModifier.Area },
            { "mass", SpellModifier.Area },
            { "eternal", SpellModifier.Eternal },
            { "dispel", SpellModifier.Dispel }
        };

        private static readonly Dictionary<string, SpellModifier> localModifierMap = new();

        public static void Initialize()
        {
            localModifierMap.Clear();
            foreach (SpellModifier modifier in Enum.GetValues(typeof(SpellModifier)))
            {
                if (modifier == SpellModifier.None)
                    continue;

                var modifierName = modifier.ToString();
                var translation = Spellwright.GetTranslation("SpellModifiers", modifierName).Value;

                var lowercaseName = modifierName.ToLower();
                localModifierMap.Add(lowercaseName, modifier);

                var lowercaseTranslation = translation.ToLower();
                if (lowercaseTranslation != lowercaseName)
                    localModifierMap.Add(lowercaseTranslation, modifier);
            }
        }

        public static void Unload()
        {
            localModifierMap.Clear();
        }

        public static SpellModifier? GetModifier(string value)
        {
            if (!modifierMap.ContainsKey(value.ToLower()))
                return null;
            else
                return modifierMap[value];
        }
    }
}
