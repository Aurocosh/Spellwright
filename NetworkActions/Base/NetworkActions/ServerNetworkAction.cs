using System;
using Terraria;
using Terraria.ID;

namespace Spellwright.Network.NetworkActions
{
    [Serializable]
    internal abstract class ServerNetworkAction : NetworkAction
    {
        protected override void HandleRouting(byte fromWho, bool fromServer)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
                DoAction();
            else
                Send(-1, fromWho);
        }
    }
}
