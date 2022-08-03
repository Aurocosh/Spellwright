using System;
using Terraria;
using Terraria.ID;

namespace Spellwright.Network.NetworkActions
{
    [Serializable]
    internal abstract class RoutedNetworkAction : NetworkAction
    {
        public byte ReceiverId;

        public Player ReceiverPlayer
        {
            get => Main.player[ReceiverId];
            set => ReceiverId = (byte)value.whoAmI;
        }

        protected RoutedNetworkAction()
        {
            ReceiverId = 0;
        }
        protected RoutedNetworkAction(byte receiverId)
        {
            ReceiverId = receiverId;
        }
        protected RoutedNetworkAction(int receiverId)
        {
            ReceiverId = (byte)receiverId;
        }

        protected override void HandleRouting(byte fromWho, bool fromServer)
        {
            if (ReceiverId == Main.myPlayer || Main.netMode == NetmodeID.SinglePlayer)
                DoAction();
            else
                Send(ReceiverId, fromWho);
        }
    }
}
