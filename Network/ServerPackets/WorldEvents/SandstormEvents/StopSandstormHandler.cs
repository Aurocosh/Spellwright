using Spellwright.Network.Base;
using Terraria.GameContent.Events;

namespace Spellwright.Network.ServerPackets.WorldEvents.SandstormEvents
{
    internal class StopSandstormHandler : ServerPacketHandler<bool>
    {
        protected override void HandleData(bool value, byte fromWho, bool fromServer)
        {
            Sandstorm.StopSandstorm();
        }
    }
}
