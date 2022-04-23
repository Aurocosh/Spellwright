using System;
using System.Collections.Generic;
using Terraria;

namespace Spellwright.Content.Spells.Storage.Base
{
    internal class CustomInventory
    {
        public List<Item> Items;

        public CustomInventory()
        {
            Items = new();
        }

        public CustomInventory(List<Item> items)
        {
            Items = items;
        }

        public Item InventoryFindItem(int itemTypeId)
        {
            bool IsValid(Item item) => item.type == itemTypeId;
            return InventoryFindItem(IsValid);
        }

        public Item InventoryFindItem(Func<Item, bool> filter)
        {
            foreach (var item in Items)
            {
                if (filter.Invoke(item) && item.stack > 0)
                    return item;
            }

            return null;
        }

        public bool ConsumeItems(int itemTypeId, int amount = 1)
        {
            bool IsValid(Item item) => item.type == itemTypeId;
            return ConsumeItems(IsValid, amount);
        }

        public bool ConsumeItems(Func<Item, bool> filter, int amount = 1)
        {
            if (amount <= 0)
                return false;

            int amountInInventory = 0;
            List<Item> fittingItems = new();
            foreach (var item in Items)
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

        public bool HasItems(int itemTypeId, int amount = 1)
        {
            bool IsValid(Item item) => item.type == itemTypeId;
            return HasItems(IsValid, amount);
        }

        public bool HasItems(Func<Item, bool> filter, int amount = 1)
        {
            if (amount <= 0)
                return false;

            int amountInInventory = 0;
            foreach (var item in Items)
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

        public int CountItems(int itemTypeId)
        {
            bool IsValid(Item item) => item.type == itemTypeId;
            return CountItems(IsValid);
        }

        public int CountItems(Func<Item, bool> filter)
        {
            int amountInInventory = 0;
            foreach (var item in Items)
            {
                if (filter.Invoke(item) && item.stack > 0)
                    amountInInventory += item.stack;
            }

            return amountInInventory;
        }
    }
}
