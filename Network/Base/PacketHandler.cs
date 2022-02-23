using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Network
{
    internal abstract class PacketHandler
    {
        internal byte HandlerType { get; set; }
        public abstract void HandlePacket(BinaryReader reader, byte fromWho, bool fromServer);

        protected PacketHandler()
        {
            HandlerType = ModNetHandler.RegisterHandler(this);
        }

        protected ModPacket GetPacket(int fromWho)
        {
            var p = Spellwright.Instance.GetPacket();
            p.Write(HandlerType);
            if (Main.netMode == NetmodeID.Server)
                p.Write((byte)fromWho);
            return p;
        }
    }
}
