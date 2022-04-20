using System;

namespace Spellwright.Content.Spells.Base.Modifiers
{
    [Flags]
    public enum SpellModifier
    {
        None = 0,
        Unlock = 1,
        Area = 2,
        Eternal = 4,
        Dispel = 8,
        Selfless = 16,
        Twofold = 32,
        Fivefold = 64,
        Tenfold = 128,
        Fiftyfold = 256,
    }
}
