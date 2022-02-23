using System.IO;
using Terraria.ModLoader;

namespace Spellwright.Network.Base
{
    internal abstract class IntSyncPacketHandler : SyncPacketHandler<int>
    {
        protected override int ReadData(BinaryReader reader) => reader.ReadInt32();
        protected override void WriteData(ModPacket packet, int data) => packet.Write(data);
    }
}
