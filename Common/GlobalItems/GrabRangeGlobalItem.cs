using Spellwright.Content.Buffs.Spells;
using Terraria;
using Terraria.ModLoader;

namespace Spellwright.Common.GlobalItems
{
    public class GrabRangeGlobalItem : GlobalItem
    {
        public override void GrabRange(Item item, Player player, ref int grabRange)
        {
            if (player.HasBuff<GreedyVortexBuff>())
                grabRange += 16 * 20;
        }
    }
}
