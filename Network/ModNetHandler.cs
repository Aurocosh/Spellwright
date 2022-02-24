﻿using Spellwright.Network.Base;
using Spellwright.Network.Dusts;
using Spellwright.Network.Handlers;
using Spellwright.Network.RoutedHandlers;
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

        // Communication
        internal static OtherPlayerHealHandler otherPlayerHealHandler = new();
        internal static OtherPlayerClearBuffsHandler otherPlayerClearBuffsHandler = new();
        internal static OtherPlayerAddBuffsHandler OtherPlayerAddBuffsHandler = new();

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