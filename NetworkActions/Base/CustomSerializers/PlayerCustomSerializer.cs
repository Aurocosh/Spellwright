using NetSerializer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Terraria;

namespace Spellwright.Network.CustomSerializers
{
    internal class PlayerCustomSerializer : IStaticTypeSerializer
    {
        public bool Handles(Type type)
        {
            return type == typeof(Player);
        }

        public IEnumerable<Type> GetSubtypes(Type type)
        {
            yield break;
        }

        public MethodInfo GetStaticWriter(Type type)
        {
            return typeof(PlayerCustomSerializer).GetMethod("WritePrimitive",
                BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.ExactBinding, null,
                new Type[] { typeof(Stream), type }, null);

        }

        public MethodInfo GetStaticReader(Type type)
        {
            return typeof(PlayerCustomSerializer).GetMethod("ReadPrimitive",
                BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.ExactBinding, null,
                new Type[] { typeof(Stream), type.MakeByRefType() }, null);
        }

        static void WritePrimitive(Stream stream, Player value)
        {
            byte playerId = (byte)value.whoAmI;
            Primitives.WritePrimitive(stream, playerId);
        }

        static void ReadPrimitive(Stream stream, out Player value)
        {
            Primitives.ReadPrimitive(stream, out byte playerId);
            value = Main.player[playerId];
        }
    }
}
