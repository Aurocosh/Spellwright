using Spellwright.Network.Base.MessageProcessors.Base;
using System;
using Terraria.ModLoader;

namespace Spellwright.Network.Base.MessageProcessors.GenericWriters
{
    internal class ArrayMessageWriter : IMessageWriter
    {
        private readonly IMessageWriter dataWriter;
        public ArrayMessageWriter(IMessageWriter writer)
        {
            dataWriter = writer;
        }

        public void Write(ModPacket packet, object data)
        {
            var array = data as Array;
            int count = array.Length;

            packet.Write(count);
            for (int i = 0; i < count; i++)
            {
                var value = array.GetValue(i);
                dataWriter.Write(packet, value);
            }
        }
    }
}
