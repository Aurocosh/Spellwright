using Spellwright.Network.NetworkActions;
using System;
using Terraria;

namespace Spellwright.Network.ServerPackets.WorldEvents.RainEvents
{
    [Serializable]
    internal class StopRainAction : ServerNetworkAction
    {
        public override void DoAction()
        {
            Main.StopRain();
        }
    }
}
