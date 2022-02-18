using System.Collections.Generic;

namespace Spellwright.Spells.SpellExtraData
{
    internal class SpellStructure
    {
        public HashSet<SpellModifier> SpellModifiers { get; }
        public string SpellName { get; }
        public string Argument { get; }

        public SpellStructure(List<SpellModifier> spellModifiers, string spellName, string argument)
        {
            SpellModifiers = new HashSet<SpellModifier>(spellModifiers);
            SpellName = spellName;
            Argument = argument;
        }
    }
}
