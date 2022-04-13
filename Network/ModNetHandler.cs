using Spellwright.DustSpawners;
using Spellwright.Network.Base;
using Spellwright.Network.RoutedHandlers;
using Spellwright.Network.Sync;
using System.Collections.Generic;
using System.IO;

namespace Spellwright.Network
{
    internal static class ModNetHandler
    {
        private static byte nextHandlerType = 0;
        internal static readonly List<PacketHandler> packetHandlers = new();

        // Value sync
        internal static SurgeOfLifeHandler surgeOfLifeHandler = new();
        internal static ReactiveArmorMaxDefenseHandler reactiveArmorMaxDefenseSync = new();
        internal static ReactiveArmorDefenseHandler reactiveArmorDefenseSync = new();
        internal static PlayerLevelHandler PlayerLevelSync = new();
        internal static DashPlayerTimerHandler dashPlayerTimerSync = new();
        internal static PermamentPlayerEffectsHandler permamentPlayerEffectsHandler = new();
        internal static EffectLevelHandler EffectLevelHandler = new();
        internal static SingleEffectLevelHandler SingleEffectLevelHandler = new();

        // Communication
        internal static OtherPlayerHealHandler otherPlayerHealHandler = new();
        internal static OtherPlayerClearBuffsHandler otherPlayerClearBuffsHandler = new();
        internal static OtherPlayerAddBuffsHandler otherPlayerAddBuffsHandler = new();
        internal static OtherPlayerRemovePermamentEffectHandler OtherPlayerRemovePermamentEffectHandler = new();
        internal static OtherPlayerAddPermamentEffectHandler otherPlayerAddPermamentEffectHandler = new();

        // Dusts
        internal static DustSpawnPacketHandler<AoeCastDustSpawner> aoeCastDustHandler = new();
        internal static DustSpawnPacketHandler<SoulDisturbanceSpawner> soulDisturbanceHandler = new();
        internal static DustSpawnPacketHandler<VortexDustSpawner> vortexDustHandler = new();
        internal static DustSpawnPacketHandler<LevelUpDustSpawner> levelUpDustHandler = new();

        // Events
        internal static StartRainHandler startRainHandler = new();

        internal static byte RegisterHandler(PacketHandler packetHandler)
        {
            packetHandlers.Add(packetHandler);
            return nextHandlerType++;
        }

        public static void HandlePacket(BinaryReader binaryReader, int fromWho)
        {
            byte handlerType = binaryReader.ReadByte();
            var handler = packetHandlers[handlerType];
            bool fromServer = fromWho == 256;
            if (fromServer)
                fromWho = binaryReader.ReadByte();
            handler.HandlePacket(binaryReader, (byte)fromWho, fromServer);
        }
    }
}
