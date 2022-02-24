using System.IO;
using Terraria;
using Terraria.ID;

namespace Spellwright.Network.Base
{
    internal class SyncData<K>
    {
        public int PlayerId { get; set; }
        public K Value { get; set; }

        public SyncData()
        {
        }

        public SyncData(int playerId, K value)
        {
            PlayerId = playerId;
            Value = value;
        }
    }

    internal abstract class SyncPacketHandler<T> : GenericPacketHandler<SyncData<T>>
    {
        public override void HandlePacket(BinaryReader reader, byte fromWho, bool fromServer)
        {
            var value = (SyncData<T>)dataReader.Read(reader);
            HandleData(value, fromWho, fromServer);
            if (Main.netMode == NetmodeID.Server)
                Send(-1, fromWho, value);
        }

        public void Sync(int toWho, int fromWho, int playerId, T data)
        {
            var syncData = new SyncData<T>(playerId, data);
            Send(toWho, fromWho, syncData);
        }
        public void Sync(int playerId, T data)
        {
            var syncData = new SyncData<T>(playerId, data);
            Send(syncData);
        }
    }
}
