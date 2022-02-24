using Spellwright.Network.Base.MessageProcessors.Base;
using System;
using Terraria.ModLoader;

namespace Spellwright.Network.Base.MessageProcessors.Writers
{
    internal class LambdaMessageWriter<T> : IMessageWriter
    {
        private readonly Action<ModPacket, T> writeFunc;

        public LambdaMessageWriter(Action<ModPacket, T> func)
        {
            writeFunc = func;
        }

        public void Write(ModPacket packet, object data)
        {
            writeFunc.Invoke(packet, (T)data);
        }
    }
}
