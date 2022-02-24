using Terraria.ModLoader;

namespace Spellwright.Network.Base.MessageProcessors.Base
{
    internal interface IMessageWriter
    {
        void Write(ModPacket packet, object data);
    }
}
