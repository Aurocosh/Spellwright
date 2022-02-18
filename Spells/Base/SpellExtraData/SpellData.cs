using System.Collections.Generic;

namespace Spellwright.Spells.SpellExtraData
{
    internal class SpellData
    {
        private readonly HashSet<SpellModifier> spellModifiers;
        private readonly List<SpellModifier> spellModifiersList;
        public string Argument { get; }

        public bool HasModifier(SpellModifier spellModifier) => spellModifiers.Contains(spellModifier);
        public IReadOnlyList<SpellModifier> GetModifiers() => spellModifiersList;

        public SpellData(SpellStructure spellStructure)
        {
            spellModifiers = spellStructure.SpellModifiers;
            spellModifiersList = new List<SpellModifier>(spellModifiers);
            Argument = spellStructure.Argument;
        }
    }
}
