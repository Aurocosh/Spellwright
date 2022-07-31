using Spellwright.Content.Spells.Base.Modifiers;

namespace Spellwright.Content.Spells.Base
{
    public class SpellData
    {
        public static readonly SpellData EmptyData = new(SpellModifier.None, "", 1f, null);

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
