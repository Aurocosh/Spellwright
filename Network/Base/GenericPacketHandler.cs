using Spellwright.Network.Base.MessageProcessors;
using Spellwright.Network.Base.MessageProcessors.Base;
using System;
using Terraria;
using Terraria.ModLoader;

namespace Spellwright.Network.Base
{
    internal abstract class GenericPacketHandler<T> : PacketHandler
    {
        protected readonly IMessageReader dataReader;
        protected readonly IMessageWriter dataWriter;

        public GenericPacketHandler()
        {
            Type typeParameterType = typeof(T);
            dataReader = MessageReaderProvider.GetReader(typeParameterType);
            dataWriter = MessageWriterProvider.GetWriter(typeParameterType);
        }

        protected abstract void HandleData(T data, byte fromWho, bool fromServer);
        public virtual void Send(int toWho, int fromWho, T data)
        {
            ModPacket packet = GetPacket(fromWho);
            dataWriter.Write(packet, data);
            packet.Send(toWho, fromWho);
        }
        public virtual void Send(T data)
        {
            ModPacket packet = GetPacket(Main.myPlayer);
            dataWriter.Write(packet, data);
            packet.Send();
        }
    }
}
