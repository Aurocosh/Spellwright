using System.IO;
using Terraria;
using Terraria.ID;

namespace Spellwright.Network.Base
{
    internal abstract class BroadcastPacketHandler<T> : GenericPacketHandler<T>
    {
        public override void HandlePacket(BinaryReader reader, byte fromWho, bool fromServer)
        {
            T value = ReadData(reader);
            if (Main.netMode == NetmodeID.Server)
                Send(-1, fromWho, value);
            else
                HandleData(value, fromWho, fromServer);
        }
    }
}
