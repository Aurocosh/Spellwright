using Spellwright.Network.Base.MessageProcessors.Base;
using System;
using System.Collections;
using System.IO;

namespace Spellwright.Network.Base.MessageProcessors.GenericReaders
{
    internal class ListMessageReader : IMessageReader
    {
        private readonly Type listType;
        private readonly IMessageReader dataReader;

        public ListMessageReader(Type type, IMessageReader reader)
        {
            listType = type;
            dataReader = reader;
        }

        public object Read(BinaryReader reader)
        {
            int count = reader.ReadInt32();

            var list = (IList)Activator.CreateInstance(listType);
            for (int i = 0; i < count; i++)
            {
                var value = dataReader.Read(reader);
                list.Add(value);
            }
            return list;
        }
    }
}
