using System;
using Terraria;
using Terraria.ID;

namespace Spellwright.Network.NetworkActions
{
    [Serializable]
    internal abstract class BroadcastNetworkAction : NetworkAction
    {
        protected override void HandleRouting(byte fromWho, bool fromServer)
        {
            if (Main.netMode != NetmodeID.Server)
                DoAction();
            if (fromWho == Main.myPlayer && Main.netMode == NetmodeID.MultiplayerClient || Main.netMode == NetmodeID.Server)
                Send(-1, fromWho);
        }
    }
}
