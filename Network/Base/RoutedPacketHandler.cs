using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Network.Base
{
    internal abstract class RoutedPacketHandler<T> : GenericPacketHandler<T>
    {
        public override void HandlePacket(BinaryReader reader, byte fromWho, bool fromServer)
        {
            int toWho = reader.ReadInt32();
            var data = (T)dataReader.Read(reader);
            if (Main.netMode == NetmodeID.MultiplayerClient && toWho == Main.myPlayer)
                HandleData(data, fromWho, fromServer);
            if (Main.netMode == NetmodeID.Server)
                Send(toWho, fromWho, data);
        }

        public override void Send(int toWho, int fromWho, T data)
        {
            ModPacket packet = GetPacket(fromWho);
            packet.Write(toWho);
            dataWriter.Write(packet, data);
            packet.Send(toWho, fromWho);
        }
        public override void Send(T data)
        {
            throw new Exception("Cannot be sent without specifing to whom it is adressed");
        }
    }
}
