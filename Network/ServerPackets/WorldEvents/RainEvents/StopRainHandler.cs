using Spellwright.Network.Base;
using Terraria;

namespace Spellwright.Network.WorldEvents
{
    internal class StopRainHandler : ServerPacketHandler<bool>
    {
        protected override void HandleData(bool value, byte fromWho, bool fromServer)
        {
            Main.StopRain();
        }
    }
}
