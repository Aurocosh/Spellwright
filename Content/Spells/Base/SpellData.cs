using Spellwright.Content.Spells.Base.Modifiers;
using System.Collections.Generic;

namespace Spellwright.Content.Spells.Base
{
    public class SpellData
    {
        private readonly HashSet<SpellModifier> spellModifiers;
        public string Argument { get; }
        public object ExtraData { get; }
        public float CostModifier { get; }
        public SpellModifier SpellModifiers { get; }

        public T GetExtraData<T>() where T : class => ExtraData as T;

        public bool HasModifier(SpellModifier spellModifier) => SpellModifiers.HasFlag(spellModifier);

        public SpellData(SpellModifier spellModifiers, string argument, float costModifier, object extraSpellData)
        {
            SpellModifiers = spellModifiers;
            Argument = argument;
            ExtraData = extraSpellData;
            CostModifier = costModifier;
        }
    }
}
