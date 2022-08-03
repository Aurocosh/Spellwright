using Spellwright.Network.NetworkActions;
using System;
using Terraria.GameContent.Events;

namespace Spellwright.Network.ServerPackets.WorldEvents.SandstormEvents
{
    [Serializable]
    internal class StartSandstormAction : ServerNetworkAction
    {
        public override void DoAction()
        {
            Sandstorm.StartSandstorm();
        }
    }
}
