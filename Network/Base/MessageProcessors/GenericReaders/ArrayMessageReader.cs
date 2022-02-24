using Spellwright.Network.Base.MessageProcessors.Base;
using System;
using System.IO;

namespace Spellwright.Network.Base.MessageProcessors.GenericReaders
{
    internal class ArrayMessageReader : IMessageReader
    {
        private readonly Type elementType;
        private readonly IMessageReader dataReader;

        public ArrayMessageReader(Type type, IMessageReader reader)
        {
            elementType = type;
            dataReader = reader;
        }

        public object Read(BinaryReader reader)
        {
            int count = reader.ReadInt32();

            var array = Array.CreateInstance(elementType, count);
            for (int i = 0; i < count; i++)
            {
                var value = dataReader.Read(reader);
                array.SetValue(value, i);
            }
            return array;
        }
    }
}
