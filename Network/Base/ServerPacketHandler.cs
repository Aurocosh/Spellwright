using System;
using System.IO;

namespace Spellwright.Network.Base
{
    internal abstract class ServerPacketHandler<T> : GenericPacketHandler<T>
    {
        public override void HandlePacket(BinaryReader reader, byte fromWho, bool fromServer)
        {
            T value = ReadData(reader);
            HandleData(value, fromWho, fromServer);
        }
        public override void Send(int toWho, int fromWho, T data)
        {
            throw new Exception("Should only be used to send messages from client to server");
        }
    }
}
