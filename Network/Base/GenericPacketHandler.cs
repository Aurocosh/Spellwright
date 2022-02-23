using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace Spellwright.Network.Base
{
    internal abstract class GenericPacketHandler<T> : PacketHandler
    {
        protected abstract void HandleData(T data, byte fromWho, bool fromServer);
        protected abstract T ReadData(BinaryReader reader);
        protected abstract void WriteData(ModPacket packet, T data);
        public virtual void Send(int toWho, int fromWho, T data)
        {
            ModPacket packet = GetPacket(fromWho);
            WriteData(packet, data);
            packet.Send(toWho, fromWho);
        }
        public virtual void Send(T data)
        {
            ModPacket packet = GetPacket(Main.myPlayer);
            WriteData(packet, data);
            packet.Send();
        }
    }
}
