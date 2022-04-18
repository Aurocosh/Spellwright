using System;

namespace Spellwright.Extensions
{
    [Flags]
    internal enum InventoryArea : short
    {
        None = 0,
        Hotbar = 1,
        Inventory = 2,
        Coins = 4,
        Ammo = 8,
        MainSlots = Hotbar | Inventory,
        All = Hotbar | Inventory | Coins | Ammo
    }
}
