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
        Dispel = 8
    }
}
