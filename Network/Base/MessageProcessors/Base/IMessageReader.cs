using System.IO;

namespace Spellwright.Network.Base.MessageProcessors.Base
{
    internal interface IMessageReader
    {
        object Read(BinaryReader reader);
    }
}
