using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace Spellwright.Network.Base.MessageProcessors
{
    internal class ExtraMessageUtils
    {
        public static Player ReadPlayer(BinaryReader binaryReader)
        {
            byte playerId = binaryReader.ReadByte();
            return Main.player[playerId];
        }

        public static void WritePlayer(ModPacket packet, Player player)
        {
            byte playerId = (byte)player.whoAmI;
            packet.Write(playerId);
        }
    }
}
