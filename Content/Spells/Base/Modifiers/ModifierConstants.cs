﻿namespace Spellwright.Content.Spells.Base.Modifiers
{
    internal static class ModifierConstants
    {
        public static readonly SpellModifier AoeModifiers = SpellModifier.Area | SpellModifier.Selfless;
        public static readonly SpellModifier EternalModifiers = SpellModifier.Eternal | SpellModifier.Dispel;
        public static readonly SpellModifier UsebleModifiers = SpellModifier.Twofold | SpellModifier.Fivefold | SpellModifier.Tenfold | SpellModifier.Fiftyfold;
    }
}
