using Microsoft.Xna.Framework;
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
        public static Point ReadPoint(BinaryReader binaryReader)
        {
            int x = binaryReader.ReadInt32();
            int y = binaryReader.ReadInt32();
            return new Point(x, y);
        }

        public static void WritePoint(ModPacket packet, Point point)
        {
            packet.Write(point.X);
            packet.Write(point.Y);
        }
    }
}
