using Spellwright.Content.Spells.Base.Modifiers;

namespace Spellwright.Content.Spells.Base
{
    public class SpellStructure
    {
        public SpellModifier SpellModifiers { get; }
        public string SpellName { get; }
        public string Argument { get; }

        public bool HasModifier(SpellModifier modifier) => SpellModifiers.HasFlag(modifier);

        public SpellStructure(SpellModifier spellModifiers, string spellName, string argument)
        {
            SpellModifiers = spellModifiers;
            SpellName = spellName;
            Argument = argument;
        }
    }
}
