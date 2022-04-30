using Spellwright.Common.Players;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Content.Spells.Base.SpellCosts.Reagent;
using Spellwright.Content.Spells.Storage.Base;
using Spellwright.Extensions;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.Storage
{
    internal class ItemVoidSpell : StorageSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 6;
            UseType = SpellType.Invocation;

            UnlockCost = new MultipleItemSpellCost()
                .WithCost(ItemID.Chest, 5)
                .WithCost(ItemID.TeleportationPotion, 5);

            CastCost = new ReagentSpellCost(ModContent.ItemType<RareSpellReagent>(), 2);
        }

        protected override bool CanAccept(Item item)
        {
            return item.stack > 0 && item.type > ItemID.None && !item.favorited;
        }

        protected override List<Item> GetStorage(Player player)
        {
            var statPlayer = player.GetModPlayer<SpellwrightStatPlayer>();
            return statPlayer.StoredItems;
        }

        protected override int StorageSize(int playerLevel)
        {
            return 50 * playerLevel;
        }

        protected override InventoryArea IncludedArea()
        {
            return InventoryArea.Inventory;
        }
    }
}