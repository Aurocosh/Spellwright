﻿using Spellwright.Content.Items.Mirrors;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.Enchant
{
    internal class WarpMirrorSpell : BindMirrorSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 2;
            UseType = SpellType.Invocation;
            itemType = ModContent.ItemType<WarpedMagicMirror>();

            SpellCost = new MultipleItemSpellCost()
                .WithCost(ModContent.ItemType<SilverMirror>(), 1)
                .WithCost(ModContent.ItemType<CommonSpellReagent>(), 2);

            UnlockCost = new MultipleItemSpellCost()
                .WithCost(ModContent.ItemType<SilverMirror>(), 1)
                .WithCost(ItemID.TeleportationPotion, 10);
        }
    }
}