using Spellwright.Network.NetworkActions;
using System;
using Terraria;

namespace Spellwright.Network.ServerPackets.WorldEvents.RainEvents
{
    [Serializable]
    internal class StartRainAction : ServerNetworkAction
    {
        public override void DoAction()
        {
            Main.StartRain();
        }
    }
}
