using Spellwright.Content.Buffs.Spells;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Common.GlobalItems
{
    public class GrabRangeGlobalItem : GlobalItem
    {
        public override void GrabRange(Item item, Player player, ref int grabRange)
        {
            if (!player.HasBuff<GreedyVortexBuff>())
                return;
            int type = item.type;
            if (type == ItemID.Heart || type == ItemID.CandyApple || type == ItemID.CandyCane)
                return;
            if (type == ItemID.Star || type == ItemID.SoulCake || type == ItemID.SugarPlum)
                return;
            if (type == ItemID.ManaCloakStar)
                return;

            grabRange += 16 * 20;
        }
    }
}
