using Spellwright.Network.Base;
using Terraria;

namespace Spellwright.Network.RoutedHandlers
{
    internal class StartRainHandler : ServerPacketHandler<bool>
    {
        protected override void HandleData(bool healValue, byte fromWho, bool fromServer)
        {
            Main.StartRain();
        }
    }
}
