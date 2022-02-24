using Spellwright.Network.Base.MessageProcessors.Base;
using System;
using System.IO;
using System.Reflection;

namespace Spellwright.Network.Base.MessageProcessors.GenericReaders
{
    internal class ClassMessageReader : IMessageReader
    {
        private readonly ConstructorInfo constructor;
        private readonly PropertyInfo[] properties;
        private readonly IMessageReader[] readers;

        public ClassMessageReader(Type classType)
        {
            constructor = classType.GetConstructor(Type.EmptyTypes);

            properties = classType.GetProperties();
            readers = new IMessageReader[properties.Length];

            for (int i = 0; i < properties.Length; i++)
            {
                PropertyInfo property = properties[i];
                var propertyType = property.PropertyType;
                readers[i] = MessageReaderProvider.GetReader(propertyType);
            }
        }

        public object Read(BinaryReader reader)
        {
            var classInstance = constructor.Invoke(Array.Empty<object>());
            for (int i = 0; i < properties.Length; i++)
            {
                PropertyInfo property = properties[i];
                IMessageReader dataReader = readers[i];
                var value = dataReader.Read(reader);
                property.SetValue(classInstance, value);
            }
            return classInstance;
        }
    }
}
