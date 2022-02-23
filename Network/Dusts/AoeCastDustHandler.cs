using Spellwright.DustSpawners;
using Spellwright.Network.Base;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace Spellwright.Network.Dusts
{
    internal class AoeCastDustHandler : BroadcastPacketHandler<AoeCastDustSpawner>
    {
        protected override void HandleData(AoeCastDustSpawner data, byte fromWho, bool fromServer)
        {
            data.Spawn();
        }

        protected override AoeCastDustSpawner ReadData(BinaryReader reader)
        {
            var spawner = new AoeCastDustSpawner
            {
                Caster = Main.player[reader.ReadByte()],
                DustType = reader.ReadInt32(),
                EffectRadius = reader.ReadByte(),
                RingDustCount = reader.ReadByte(),
                EffectDustCount = reader.ReadByte(),
                AffectedPlayers = new Player[reader.ReadByte()]
            };

            int playerCount = spawner.AffectedPlayers.Length;
            for (int i = 0; i < playerCount; i++)
                spawner.AffectedPlayers[i] = Main.player[reader.ReadByte()];

            return spawner;
        }

        protected override void WriteData(ModPacket packet, AoeCastDustSpawner data)
        {
            packet.Write((byte)data.Caster.whoAmI);
            packet.Write(data.DustType);
            packet.Write(data.EffectRadius);
            packet.Write(data.RingDustCount);
            packet.Write(data.EffectDustCount);
            packet.Write((byte)data.AffectedPlayers.Length);
            foreach (var player in data.AffectedPlayers)
                packet.Write((byte)player.whoAmI);
        }
    }
}
