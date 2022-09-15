using Spellwright.Content.Items;
using Spellwright.Content.Items.SpellTomes;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Common.System
{
    internal class ChestItemSpawnSystem : ModSystem
    {
        // We can use PostWorldGen for world generation tasks that don't need to happen between vanilla world generation steps.
        public override void PostWorldGen()
        {
            PlaceItemsInChests(ModContent.ItemType<SpellResonator>(), 3, new int[] { 0, 1 });
            PlaceItemsInChests(ModContent.ItemType<BeginnerSpellTome>(), 30, new int[] { 0, 1 });
            PlaceItemsInChests(ModContent.ItemType<AdvancedSpellTome>(), 25, new int[] { 0, 1 });
            PlaceItemsInChests(ModContent.ItemType<SupremeSpellTome>(), 15, new int[] { 0, 1, 2 });
        }

        private static void PlaceItemsInChests(int itemTypeId, int count, IEnumerable<int> allowedChestTypes)
        {
            var allowedChestsHash = new HashSet<int>(allowedChestTypes);
            var visitedChests = new HashSet<Chest>();
            int itemsToPlace = Math.Min(count, 1000);
            int tryLimit = 2000;

            while (itemsToPlace > 0 && tryLimit-- > 0)
            {
                int chestIndex = Main.rand.Next(0, 999);
                Chest chest = Main.chest[chestIndex];
                if (chest != null && !visitedChests.Contains(chest))
                {
                    var chestTile = Main.tile[chest.x, chest.y];
                    int chestType = chestTile.TileFrameX / 36;
                    if (chestTile.TileType == TileID.Containers && allowedChestsHash.Contains(chestType))
                    {
                        int freeItemSlot = FindEmptySlotInChest(chest.item);
                        if (freeItemSlot > 0)
                        {
                            chest.item[freeItemSlot].SetDefaults(itemTypeId);
                            itemsToPlace--;
                        }
                    }
                    visitedChests.Add(chest);
                }
            }
        }

        private static int FindEmptySlotInChest(Item[] chestSlots)
        {
            for (int i = 0; i < 40; i++)
            {
                if (chestSlots[i].type == ItemID.None)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
