namespace Spellwright.Content.Spells.Base.Modifiers
{
    internal static class ModifierConstants
    {
        public static readonly SpellModifier AoeModifiers = SpellModifier.IsAoe | SpellModifier.IsSelfless;
        public static readonly SpellModifier EternalModifiers = SpellModifier.IsEternal | SpellModifier.IsDispel;
        public static readonly SpellModifier UsebleModifiers = SpellModifier.IsTwofold | SpellModifier.IsFivefold | SpellModifier.IsTenfold | SpellModifier.IsFiftyfold;
    }
}
