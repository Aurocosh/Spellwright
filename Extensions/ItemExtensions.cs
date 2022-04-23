using Terraria;

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
    }
}
