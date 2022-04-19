using System;

namespace Spellwright.Content.Spells.Base.Modifiers
{
    [Flags]
    public enum SpellModifier
    {
        None = 0,
        IsAoe = 1,
        IsEternal = 2,
        IsDispel = 4,
        IsSelfless = 8,
        IsTwofold = 16,
        IsFivefold = 32,
        IsTenfold = 64,
        IsFiftyfold = 128,
    }
}
