using Spellwright.ExecutablePackets.Broadcast.DustSpawners;
using Spellwright.ExecutablePackets.ToServer;
using Spellwright.Network.Base;
using Spellwright.Network.Base.Executable;
using Spellwright.Network.RoutedHandlers;
using Spellwright.Network.RoutedHandlers.Buffs;
using Spellwright.Network.RoutedHandlers.PermanentEffects;
using Spellwright.Network.RoutedHandlers.StatSync;
using Spellwright.Network.ServerPackets.WorldEvents.RainEvents;
using Spellwright.Network.ServerPackets.WorldEvents.SandstormEvents;
using Spellwright.Network.Sync;
using Spellwright.Network.Sync.EffectLevels;
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
        internal static PermanentPlayerEffectsHandler permanentPlayerEffectsHandler = new();
        internal static EffectLevelHandler EffectLevelHandler = new();
        internal static SingleEffectLevelHandler SingleEffectLevelHandler = new();

        // Communication
        internal static OtherPlayerHealHandler otherPlayerHealHandler = new();
        internal static OtherPlayerClearBuffsHandler otherPlayerClearBuffsHandler = new();
        internal static OtherPlayerAddBuffsHandler otherPlayerAddBuffsHandler = new();
        internal static OtherPlayerRemovePermanentEffectHandler OtherPlayerRemovePermanentEffectHandler = new();
        internal static OtherPlayerAddPermanentEffectHandler otherPlayerAddPermanentEffectHandler = new();

        // Dusts
        internal static BroadcastExecutablePacketHandler<AoeCastDustSpawner> aoeCastDustHandler = new();
        internal static BroadcastExecutablePacketHandler<SoulDisturbanceSpawner> soulDisturbanceHandler = new();
        internal static BroadcastExecutablePacketHandler<VortexDustSpawner> vortexDustHandler = new();
        internal static BroadcastExecutablePacketHandler<LevelUpDustSpawner> levelUpDustHandler = new();
        internal static BroadcastExecutablePacketHandler<CastDustSpawner> castDustHandler = new();
        internal static BroadcastExecutablePacketHandler<HerbAoeDustSpawner> HerbAoeDustHandler = new();

        // Events
        internal static StartRainHandler StartRainHandler = new();
        internal static StopRainHandler StopRainHandler = new();
        internal static StartSandstormHandler StartSandstormHandler = new();
        internal static StopSandstormHandler StopSandstormHandler = new();

        // Commands
        internal static ServerExecutablePacketHandler<AreaHerbAndTreeGrower> AreaHerbAndTreeGrowerHandler = new();

        // Stat set routed
        internal static MetaBoostCountSetHandler MetaBoostCountSetHandler = new();

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
