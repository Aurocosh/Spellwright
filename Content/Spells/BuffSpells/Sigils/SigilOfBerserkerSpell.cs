﻿using Spellwright.Content.Buffs.Spells.Sigils;
using Spellwright.Content.Spells.Base;
using Spellwright.Util;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.BuffSpells.Sigils
{
    internal class SigilOfBerserkerSpell : SigilBuffSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 6;
            AddEffect(ModContent.BuffType<SigilOfBerserkerBuff>(), (playerLevel) => UtilTime.MinutesToTicks(10 + 2 * playerLevel));

            RemoveApplicableModifier(SpellModifier.IsAoe);

            AddApplicableModifier(SpellModifier.IsDispel);
            AddApplicableModifier(SpellModifier.IsEternal);
        }
    }
}
