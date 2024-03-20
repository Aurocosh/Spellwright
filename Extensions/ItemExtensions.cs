using Terraria;
using Terraria.ID;

namespace Spellwright.Extensions
{
    internal static class ItemExtensions
    {
        public static bool IsTheSameAs(this Item item, Item compareItem)
        {
            if (item.netID == compareItem.netID)
                return item.type == compareItem.type;
            return false;
        }

        public static bool IsValidItem(this Item item)
        {
            return item.type != ItemID.None && item.stack > 0;
        }

        public static void Consume(this Item item, int amount)
        {
            item.stack -= amount;
            if (item.stack <= 0)
                item.TurnToAir();
        }
    }
}
