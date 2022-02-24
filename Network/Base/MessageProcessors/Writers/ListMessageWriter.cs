using Spellwright.Network.Base.MessageProcessors.Base;
using System.Collections;
using Terraria.ModLoader;

namespace Spellwright.Network.Base.MessageProcessors.Writers
{
    internal class ListMessageWriter : IMessageWriter
    {
        private readonly IMessageWriter dataWriter;
        public ListMessageWriter(IMessageWriter writer)
        {
            dataWriter = writer;
        }

        public void Write(ModPacket packet, object data)
        {
            var list = data as IList;
            int count = list.Count;

            packet.Write(count);
            for (int i = 0; i < count; i++)
            {
                var value = list[i];
                dataWriter.Write(packet, value);
            }
        }
    }
}
