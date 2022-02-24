using Spellwright.Network.Base.MessageProcessors.Base;
using System;
using System.Reflection;
using Terraria.ModLoader;

namespace Spellwright.Network.Base.MessageProcessors.GenericWriters
{
    internal class MethodMessageWriter<T> : IMessageWriter
    {
        public delegate void DelegateWriter(ModPacket packet, T data);
        private readonly DelegateWriter delegateWriter;

        public MethodMessageWriter(Type type, MethodInfo methodInfo)
        {
            delegateWriter = (DelegateWriter)Delegate.CreateDelegate(typeof(DelegateWriter), null, methodInfo, true);
        }

        public void Write(ModPacket packet, object data)
        {
            delegateWriter(packet, (T)data);
        }
    }
}
