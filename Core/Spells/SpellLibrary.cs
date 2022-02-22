using Spellwright.Content.Spells.Base;
using System;
using System.Collections.Generic;

namespace Spellwright.Core.Spells
{
    internal class SpellLibrary
    {
        private static readonly Dictionary<string, ModSpell> spellIncantationMap = new();
        internal static void SetSpellIncantation(string newIncantation, ModSpell modSpell)
        {
            if (spellIncantationMap.TryGetValue(newIncantation, out var existingSpell))
                if (existingSpell != modSpell)
                    throw new Exception($"Spell incantation conflict. Two spells have identical incantation: {newIncantation}");
            spellIncantationMap.Add(newIncantation.ToLower(), modSpell);
        }
        internal static ModSpell GetSpellByIncantation(string incantation)
        {
            if (!spellIncantationMap.TryGetValue(incantation.ToLower(), out ModSpell spell))
                return null;
            return spell;
        }
    }
}
