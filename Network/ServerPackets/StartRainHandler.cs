using Spellwright.Network.Base;
using Terraria;

namespace Spellwright.Network.ServerPackets
{
    internal class GrowHerbsAndTreesHandler : ServerPacketHandler<bool>
    {
        protected override void HandleData(bool healValue, byte fromWho, bool fromServer)
        {
            Main.StartRain();
        }
    }
}
