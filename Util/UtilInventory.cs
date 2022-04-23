using System;
using System.Collections.Generic;
using Terraria;

namespace Spellwright.Util
{
    internal class UtilInventory
    {
        public static Item InventoryFindItem(IEnumerable<Item> items, int itemTypeId)
        {
            bool IsValid(Item item) => item.type == itemTypeId;
            return InventoryFindItem(items, IsValid);
        }

        public static Item InventoryFindItem(IEnumerable<Item> items, Func<Item, bool> filter)
        {
            foreach (var item in items)
            {
                if (filter.Invoke(item) && item.stack > 0)
                    return item;
            }

            return null;
        }

        public static bool ConsumeItems(IEnumerable<Item> items, int itemTypeId, int amount = 1)
        {
            bool IsValid(Item item) => item.type == itemTypeId;
            return ConsumeItems(items, IsValid, amount);
        }

        public static bool ConsumeItems(IEnumerable<Item> items, Func<Item, bool> filter, int amount = 1)
        {
            if (amount <= 0)
                return false;

            int amountInInventory = 0;
            List<Item> fittingItems = new();
            foreach (var item in items)
            {
                if (filter.Invoke(item) && item.stack > 0)
                {
                    fittingItems.Add(item);
                    amountInInventory += item.stack;
                    if (amountInInventory > amount)
                        break;
                }
            }

            if (amountInInventory < amount)
                return false;

            foreach (Item item in fittingItems)
            {
                int amountToSub = Math.Min(amount, item.stack);
                item.stack -= amountToSub;
                if (item.stack == 0)
                    item.TurnToAir();
                amount -= amountToSub;
                if (amount == 0)
                    break;
            }

            return true;
        }

        public static bool HasItems(IEnumerable<Item> items, int itemTypeId, int amount = 1)
        {
            bool IsValid(Item item) => item.type == itemTypeId;
            return HasItems(items, IsValid, amount);
        }

        public static bool HasItems(IEnumerable<Item> items, Func<Item, bool> filter, int amount = 1)
        {
            if (amount <= 0)
                return false;

            int amountInInventory = 0;
            foreach (var item in items)
            {
                if (filter.Invoke(item) && item.stack > 0)
                {
                    amountInInventory += item.stack;
                    if (amountInInventory > amount)
                        return true;
                }
            }

            return false;
        }

        public static int CountItems(IEnumerable<Item> items, int itemTypeId)
        {
            bool IsValid(Item item) => item.type == itemTypeId;
            return CountItems(items, IsValid);
        }

        public static int CountItems(IEnumerable<Item> items, Func<Item, bool> filter)
        {
            int amountInInventory = 0;
            foreach (var item in items)
            {
                if (filter.Invoke(item) && item.stack > 0)
                    amountInInventory += item.stack;
            }

            return amountInInventory;
        }
    }
}
