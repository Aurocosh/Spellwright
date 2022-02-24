using Spellwright.Network.Base.MessageProcessors.Base;
using Spellwright.Network.Base.MessageProcessors.Readers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Terraria;

namespace Spellwright.Network.Base.MessageProcessors
{
    internal static class MessageReaderProvider
    {
        private static readonly Dictionary<Type, IMessageReader> readers = new();

        static MessageReaderProvider()
        {
            Type binaryType = typeof(BinaryReader);
            AddReader<bool>(binaryType, "ReadBoolean");
            AddReader<byte>(binaryType, "ReadByte");
            AddReader<char>(binaryType, "ReadChar");
            AddReader<decimal>(binaryType, "ReadDecimal");
            AddReader<double>(binaryType, "ReadDouble");
            AddReader<Half>(binaryType, "ReadHalf");
            AddReader<short>(binaryType, "ReadInt16");
            AddReader<int>(binaryType, "ReadInt32");
            AddReader<long>(binaryType, "ReadInt64");
            AddReader<sbyte>(binaryType, "ReadSByte");
            AddReader<float>(binaryType, "ReadSingle");
            AddReader<string>(binaryType, "ReadString");
            AddReader<ushort>(binaryType, "ReadUInt16");
            AddReader<uint>(binaryType, "ReadUInt32");
            AddReader<ulong>(binaryType, "ReadUInt64");

            Type extraType = typeof(ExtraMessageUtils);
            AddStaticReader<Player>(extraType, "ReadPlayer");
        }

        private static void AddReader<T>(Type type, string methodName)
        {
            Type argumentType = typeof(T);
            MethodInfo methodInfo = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance);
            var reader = new MethodMessageReader<T>(type, methodInfo);
            readers.Add(argumentType, reader);
        }

        private static void AddStaticReader<T>(Type type, string methodName)
        {
            Type argumentType = typeof(T);
            MethodInfo methodInfo = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static);
            var reader = new MethodMessageReader<T>(type, methodInfo);
            readers.Add(argumentType, reader);
        }

        public static IMessageReader GetReader(Type type)
        {
            if (readers.TryGetValue(type, out var reader))
                return reader;
            if (type.IsArray)
            {
                var elementType = type.GetElementType();
                var elementReader = GetReader(elementType);
                return new ArrayMessageReader(elementType, elementReader);
            }
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                var elementType = type.GetGenericArguments()[0];
                var elementReader = GetReader(elementType);
                return new ListMessageReader(type, elementReader);
            }
            if (type.IsClass)
                return new ClassMessageReader(type);

            throw new Exception("Unsupported reader type requested");
        }
    }
}
