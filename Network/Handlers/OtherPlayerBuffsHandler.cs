using Spellwright.Data;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace Spellwright.Network.Handlers
{
    internal class OtherPlayerBuffsHandler : RoutedPacketHandler<BuffData[]>
    {
        protected override void HandleData(BuffData[] data, byte fromWho, bool fromServer)
        {
            Player player = Main.LocalPlayer;
            foreach (var buffData in data)
                player.AddBuff(buffData.Type, buffData.Duration);
        }

        protected override BuffData[] ReadData(BinaryReader reader)
        {
            int dataCount = reader.ReadInt32();
            var buffData = new BuffData[dataCount];
            for (int i = 0; i < dataCount; i++)
            {
                int type = reader.ReadInt32();
                int duration = reader.ReadInt32();
                buffData[i] = new BuffData(type, duration);
            }
            return buffData;
        }

        protected override void WriteData(ModPacket packet, BuffData[] data)
        {
            packet.Write(data.Length);
            for (int i = 0; i < data.Length; i++)
            {
                var buffData = data[i];
                packet.Write(buffData.Type);
                packet.Write(buffData.Duration);
            }
        }
    }
}
