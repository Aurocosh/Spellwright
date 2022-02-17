using System;
using System.Collections.Generic;
using Terraria;

namespace Spellwright.Extensions
{
    internal static class PlayerExtensions
    {
        public static bool ConsumeItems(this Player player, int itemTypeId, int amount = 1)
        {
            if (amount <= 0)
                return false;

            int slotCount = player.inventory.Length;
            //slotCount = 49;

            List<Item> fittingItems = new();

            int amountInInventory = 0;
            for (int i = 0; i < slotCount; i++)
            {
                Item item = player.inventory[i];
                if (item.type == itemTypeId && item.stack > 0)
                {
                    amountInInventory += item.stack;
                    fittingItems.Add(item);
                }
                if (amountInInventory > amount)
                    break;
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
    }
}
