using Microsoft.Xna.Framework;
using Spellwright.Network.Base.MessageProcessors.Base;
using Spellwright.Network.Base.MessageProcessors.GenericWriters;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace Spellwright.Network.Base.MessageProcessors
{
    internal static class MessageWriterProvider
    {
        private static readonly Dictionary<Type, IMessageWriter> writers = new();

        static MessageWriterProvider()
        {
            Type packetType = typeof(ModPacket);
            AddWriter<bool>(packetType, "Write");
            AddWriter<byte>(packetType, "Write");
            AddWriter<char>(packetType, "Write");
            AddWriter<decimal>(packetType, "Write");
            AddWriter<double>(packetType, "Write");
            AddWriter<Half>(packetType, "Write");
            AddWriter<short>(packetType, "Write");
            AddWriter<int>(packetType, "Write");
            AddWriter<long>(packetType, "Write");
            AddWriter<sbyte>(packetType, "Write");
            AddWriter<float>(packetType, "Write");
            AddWriter<string>(packetType, "Write");
            AddWriter<ushort>(packetType, "Write");
            AddWriter<uint>(packetType, "Write");
            AddWriter<ulong>(packetType, "Write");

            Type extraType = typeof(ExtraMessageUtils);
            AddStaticWriter<Player>(extraType, "WritePlayer");
            AddStaticWriter<Point>(extraType, "WritePoint");

            Type utilsType = typeof(Utils);
            AddStaticWriter<Vector2>(utilsType, "WriteVector2");
            AddStaticWriter<Color>(utilsType, "WriteRGB");
        }

        private static void AddWriter<T>(Type type, string methodName)
        {
            Type argumentType = typeof(T);
            MethodInfo methodInfo = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance, new Type[] { argumentType });
            var writer = new MethodMessageWriter<T>(type, methodInfo);
            writers.Add(argumentType, writer);
        }
        private static void AddStaticWriter<T>(Type type, string methodName)
        {
            Type argumentType = typeof(T);
            MethodInfo methodInfo = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static, new Type[] { typeof(ModPacket), argumentType });
            var writer = new MethodMessageWriter<T>(type, methodInfo);
            writers.Add(argumentType, writer);
        }

        public static IMessageWriter GetWriter(Type type)
        {
            if (writers.TryGetValue(type, out var writer))
                return writer;
            if (type.IsArray)
            {
                var elementType = type.GetElementType();
                var elementWriter = GetWriter(elementType);
                return new ArrayMessageWriter(elementWriter);
            }
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                var elementType = type.GetGenericArguments()[0];
                var elementWriter = GetWriter(elementType);
                return new ListMessageWriter(elementWriter);
            }
            if (type.IsClass)
                return new ClassMessageWriter(type);

            throw new Exception("Unsupported writer type requested");
        }
    }
}
