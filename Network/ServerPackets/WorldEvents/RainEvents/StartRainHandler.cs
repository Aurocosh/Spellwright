using Spellwright.Network.Base;
using Terraria;

namespace Spellwright.Network.ServerPackets.WorldEvents.RainEvents
{
    internal class StartRainHandler : ServerPacketHandler<bool>
    {
        protected override void HandleData(bool healValue, byte fromWho, bool fromServer)
        {
            Main.StartRain();
        }
    }
}
