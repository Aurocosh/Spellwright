﻿using Spellwright.Content.Buffs.Spells.Utility;
using Spellwright.Content.Spells.Base.Modifiers;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.BuffSpells.Utility
{
    internal class ForceOfCreationSpell : BuffSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 5;
            AddEffect(ModContent.BuffType<ForceOfCreationBuff>(), (playerLevel) => UtilTime.MinutesToTicks((int)(2f * playerLevel)));
            AddApplicableModifier(ModifierConstants.EternalModifiers);
        }
    }
}