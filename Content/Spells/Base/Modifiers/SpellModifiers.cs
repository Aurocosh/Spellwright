using System;

namespace Spellwright.Content.Spells.Base.Modifiers
{
    [Flags]
    public enum SpellModifier
    {
        None = 0,
        IsUnlock = 1,
        IsAoe = 2,
        IsEternal = 4,
        IsDispel = 8,
        IsSelfless = 16,
        IsTwofold = 32,
        IsFivefold = 64,
        IsTenfold = 128,
        IsFiftyfold = 256,
    }
}
