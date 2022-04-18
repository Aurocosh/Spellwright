using Spellwright.Network.Base;
using Terraria.GameContent.Events;

namespace Spellwright.Network.ServerPackets.WorldEvents.SandstormEvents
{
    internal class StartSandstormHandler : ServerPacketHandler<bool>
    {
        protected override void HandleData(bool value, byte fromWho, bool fromServer)
        {
            Sandstorm.StartSandstorm();
        }
    }
}
