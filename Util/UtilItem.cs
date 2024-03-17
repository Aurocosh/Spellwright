using Spellwright.Extensions;
using System;
using Terraria;
using Terraria.ID;

namespace Spellwright.Util
{
    internal class UtilItem
    {
        public static bool MergeItem(Item fromItem, Item toItem)
        {
            if (toItem.type > ItemID.None && toItem.stack < toItem.maxStack && fromItem.IsTheSameAs(toItem))
            {
                int unitsToMove = Math.Min(toItem.maxStack - toItem.stack, fromItem.stack);
                toItem.stack += unitsToMove;
                fromItem.stack -= unitsToMove;
                if(fromItem.stack == 0)
                    fromItem.TurnToAir();
                return true;
            }
            return false;
        }
    }
}
