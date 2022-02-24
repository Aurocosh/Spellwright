﻿using Spellwright.Content.Buffs.Spells;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.BuffSpells
{
    internal class WillOfTorchGodSpell : BuffSpell
    {
        public override void SetStaticDefaults()
        {
            int buffId = ModContent.BuffType<WillOfTorchGodBuff>();
            AddEffect(buffId, (playerLevel) => UtilTime.MinutesToTicks(10 + 2 * playerLevel));
            RemoveApplicableModifier(SpellModifier.IsAoe);
        }
    }
}