﻿using Spellwright.Content.Buffs.Spells;
using Spellwright.Content.Spells.Base.Modifiers;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.BuffSpells
{
    internal class WillOfTorchGodSpell : BuffSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 1;
            int buffId = ModContent.BuffType<WillOfTorchGodBuff>();
            AddEffect(buffId, (playerLevel) => UtilTime.MinutesToTicks(10 + 2 * playerLevel));
            RemoveApplicableModifier(SpellModifier.Area);
            RemoveApplicableModifier(SpellModifier.Selfless);

            UnlockCost = new SingleItemSpellCost(ItemID.TorchGodsFavor);
        }
    }
}
