using Spellwright.Network.Base.MessageProcessors.Base;
using System;
using System.IO;
using System.Reflection;

namespace Spellwright.Network.Base.MessageProcessors.Readers
{
    internal class MethodMessageReader<T> : IMessageReader
    {
        public delegate T DelegateReader(BinaryReader reader);
        private readonly DelegateReader delegateReader;

        public MethodMessageReader(Type type, MethodInfo methodInfo)
        {
            delegateReader = (DelegateReader)Delegate.CreateDelegate(typeof(DelegateReader), null, methodInfo, true);
        }

        public object Read(BinaryReader reader)
        {
            return delegateReader(reader);
        }
    }
}
