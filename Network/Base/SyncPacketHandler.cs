using System.IO;
using Terraria;
using Terraria.ID;

namespace Spellwright.Network
{
    internal abstract class SyncPacketHandler<T> : GenericPacketHandler<T>
    {
        public override void HandlePacket(BinaryReader reader, byte fromWho, bool fromServer)
        {
            T value = ReadData(reader);
            HandleData(value, fromWho, fromServer);
            if (Main.netMode == NetmodeID.Server)
                Send(-1, fromWho, value);
        }
    }
}
