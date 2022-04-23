﻿using Spellwright.Common.Players;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Extensions;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.Storage
{
    internal sealed class PotionVoidSpell : StorageSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 4;
            UseType = SpellType.Invocation;

            UnlockCost = new MultipleItemSpellCost()
                .WithCost(ItemID.Bottle, 5)
                .WithCost(ItemID.TeleportationPotion, 5);

            SpellCost = new ReagentSpellCost(ModContent.ItemType<CommonSpellReagent>(), 4);
        }

        protected override bool CanAccept(Item item)
        {
            return item.stack > 0 && item.type > ItemID.None && item.buffType > 0 && item.DamageType != DamageClass.Summon;
        }

        protected override List<Item> GetStorage(Player player)
        {
            var statPlayer = player.GetModPlayer<SpellwrightStatPlayer>();
            return statPlayer.PotionItems;
        }

        protected override int StorageSize(int playerLevel)
        {
            return playerLevel;
        }

        protected override InventoryArea IncludedArea()
        {
            return InventoryArea.Inventory;
        }
    }
}