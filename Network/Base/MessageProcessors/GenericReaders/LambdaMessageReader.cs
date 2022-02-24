using Spellwright.Network.Base.MessageProcessors.Base;
using System;
using System.IO;

namespace Spellwright.Network.Base.MessageProcessors.GenericReaders
{
    internal class LambdaMessageReader<T> : IMessageReader
    {
        private readonly Func<BinaryReader, T> readFunc;
        public LambdaMessageReader(Func<BinaryReader, T> func)
        {
            readFunc = func;
        }

        public object Read(BinaryReader reader)
        {
            return readFunc.Invoke(reader);
        }
    }
}
