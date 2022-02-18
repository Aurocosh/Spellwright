using Spellwright.Content.Buffs;
using Terraria;
using Terraria.ModLoader;

namespace Spellwright.GlobalItems
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
