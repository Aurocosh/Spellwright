﻿using Spellwright.Content.Buffs.Spells;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base.Modifiers;
using Spellwright.Content.Spells.Base.SpellCosts;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Content.Spells.Base.SpellCosts.Reagent;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.BuffSpells
{
    internal class BattlecrySpell : BuffSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 2;
            AddEffect(ModContent.BuffType<BattlecryBuff>(), (playerLevel) => UtilTime.MinutesToTicks(10 + 2 * playerLevel));
            RemoveApplicableModifier(SpellModifier.Area);
            RemoveApplicableModifier(SpellModifier.Selfless);

            AddApplicableModifier(ModifierConstants.EternalModifiers);
            castSound = new LegacySoundStyle(SoundID.Roar, 0);

            var unlockCost = new OptionalSpellCost();
            unlockCost.AddOptionalCost(new SingleItemSpellCost(ItemID.Vertebrae, 25));
            unlockCost.AddOptionalCost(new SingleItemSpellCost(ItemID.RottenChunk, 25));
            UnlockCost = unlockCost;

            CastCost = new ReagentSpellCost(ModContent.ItemType<CommonSpellReagent>(), 50);
        }
    }
}
