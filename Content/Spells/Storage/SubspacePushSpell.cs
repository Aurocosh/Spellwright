using Spellwright.Common.Players;
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
    internal class SubspacePushSpell : ModSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 6;
            UseType = SpellType.Invocation;

            UnlockCost = new MultipleItemSpellCost()
                .WithCost(ItemID.SoulofLight, 5)
                .WithCost(ItemID.TeleportationPotion, 5);

            SpellCost = new SingleItemSpellCost(ModContent.ItemType<RareSpellReagent>(), 2);
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            var spellPlayer = player.GetModPlayer<SpellwrightPlayer>();
            var statPlayer = player.GetModPlayer<SpellwrightStatPlayer>();

            int maxStorageSize = 50 * spellPlayer.PlayerLevel;
            List<Item> storedItems = statPlayer.StoredItems;

            bool storedAtLeastOne = false;
            foreach (int i in player.GetInventoryIndexes(InventoryArea.MainSlots, true))
            {
                if (storedItems.Count >= maxStorageSize)
                    break;

                Item item = player.inventory[i];
                if (item.type != ItemID.None && item.stack > 0 && !item.favorited)
                {
                    storedItems.Add(item);
                    player.inventory[i] = new Item();
                    storedAtLeastOne = true;
                }
            }

            return storedAtLeastOne;
        }
    }
}