using Spellwright.Network.Base;
using Spellwright.Network.Dusts;
using Spellwright.Network.Handlers;
using System.Collections.Generic;
using System.IO;

namespace Spellwright.Network
{
    internal static class ModNetHandler
    {
        private static byte nextHandlerType = 0;
        private static readonly List<PacketHandler> packetHandlers = new();

        // Value sync
        internal static ReactiveArmorDefenseHandler reactiveArmorDefenseSync = new();
        internal static ReactiveArmorMaxDefenseHandler reactiveArmorMaxDefenseSync = new();
        internal static DashPlayerTimerHandler dashPlayerTimerSync = new();
        internal static PlayerLevelHandler PlayerLevelSync = new();

        // Communication
        internal static OtherPlayerBuffsHandler OtherPlayerBuffsHandler = new();

        // Dusts
        internal static AoeCastDustHandler aoeCastDustHandler = new();

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
