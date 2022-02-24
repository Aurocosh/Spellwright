using Spellwright.Network.Base.MessageProcessors.Base;
using System;
using System.Reflection;
using Terraria.ModLoader;

namespace Spellwright.Network.Base.MessageProcessors.GenericWriters
{
    internal class ClassMessageWriter : IMessageWriter
    {
        private readonly PropertyInfo[] properties;
        private readonly IMessageWriter[] writers;

        public ClassMessageWriter(Type classType)
        {
            properties = classType.GetProperties();
            writers = new IMessageWriter[properties.Length];

            for (int i = 0; i < properties.Length; i++)
            {
                PropertyInfo property = properties[i];
                var propertyType = property.PropertyType;
                writers[i] = MessageWriterProvider.GetWriter(propertyType);
            }
        }
        public void Write(ModPacket packet, object data)
        {
            for (int i = 0; i < properties.Length; i++)
            {
                PropertyInfo property = properties[i];
                IMessageWriter writer = writers[i];
                var value = property.GetValue(data);
                writer.Write(packet, value);
            }
        }
    }
}
