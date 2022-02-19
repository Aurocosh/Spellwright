using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;

namespace Spellwright.Extensions
{
    internal static class PlayerExtensions
    {
        public static Rectangle GetAreaRect(this Player player, int radius)
        {
            var playerCenter = player.Center;
            return new Rectangle((int)playerCenter.X - radius, (int)playerCenter.Y - radius, player.width + radius * 2, player.height + radius * 2);
        }
        public static Rectangle GetAreaRect(this Player player, int widthHalf, int heightHalf)
        {
            var playerCenter = player.Center;
            return new Rectangle((int)playerCenter.X - widthHalf, (int)playerCenter.Y - heightHalf, player.width + widthHalf * 2, player.height + heightHalf * 2);
        }

        public static bool ConsumeItems(this Player player, int itemTypeId, int amount = 1, bool reverseOrder = false)
        {
            bool IsValid(Item item) => item.type == itemTypeId;
            return ConsumeItems(player, IsValid, amount, reverseOrder);
        }

        public static bool ConsumeItems(this Player player, Func<Item, bool> filter, int amount = 1, bool reverseOrder = false)
        {
            if (amount <= 0)
                return false;

            int startingIndex = 0;
            int limit = 58;
            int step = 1;

            if (reverseOrder)
            {
                startingIndex = 57;
                limit = -1;
                step = -1;
            }

            List<Item> fittingItems = new();

            int amountInInventory = 0;
            for (int i = startingIndex; i < limit; i += step)
            {
                Item item = player.inventory[i];
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

        public static bool HasItems(this Player player, int itemTypeId, int amount = 1, bool reverseOrder = false)
        {
            bool IsValid(Item item) => item.type == itemTypeId;
            return HasItems(player, IsValid, amount, reverseOrder);
        }

        public static bool HasItems(this Player player, Func<Item, bool> filter, int amount = 1, bool reverseOrder = false)
        {
            if (amount <= 0)
                return false;

            int startingIndex = 0;
            int limit = 58;
            int step = 1;

            if (reverseOrder)
            {
                startingIndex = 57;
                limit = -1;
                step = -1;
            }

            int amountInInventory = 0;
            for (int i = startingIndex; i < limit; i += step)
            {
                Item item = player.inventory[i];
                if (filter.Invoke(item) && item.stack > 0)
                {
                    amountInInventory += item.stack;
                    if (amountInInventory > amount)
                        return true;
                }
            }

            return false;
        }
    }
}
