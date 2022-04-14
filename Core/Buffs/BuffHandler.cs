using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Core.Buffs
{
    internal class BuffHandler
    {
        public static void UpdateBuff(int buffId, Player player)
        {
            int virtualBuffIndex = 0;

            switch (buffId)
            {
                case BuffID.Spelunker:
                    player.findTreasure = true;
                    break;
                case BuffID.Shine:
                    Lighting.AddLight((int)(player.position.X + player.width / 2) / 16, (int)(player.position.Y + player.height / 2) / 16, 0.8f, 0.95f, 1f);
                    break;
                case BuffID.Hunter:
                    player.detectCreature = true;
                    break;
            }

            if (virtualBuffIndex == 0)
                BuffLoader.Update(buffId, player, ref virtualBuffIndex);
        }
    }
}
