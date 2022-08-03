using System;
using Terraria;
using Terraria.ID;

namespace Spellwright.Network.NetworkActions
{
    [Serializable]
    internal abstract class SyncNetworkAction : NetworkAction
    {
        private byte senderId;

        public int SenderId
        {
            get => senderId;
            set => senderId = (byte)value;
        }
        public int ReceiverId { get; set; }

        protected SyncNetworkAction(byte senderId)
        {
            SenderId = senderId;
            ReceiverId = -1;
        }
        protected SyncNetworkAction(int senderId)
        {
            SenderId = (byte)senderId;
            ReceiverId = -1;
        }

        protected SyncNetworkAction(int senderId, int receiverId)
        {
            SenderId = (byte)senderId;
            ReceiverId = receiverId;
        }

        protected SyncNetworkAction(byte senderId, int receiverId, Player senderPlayer) : this(senderId)
        {
            ReceiverId = receiverId;
            SenderPlayer = senderPlayer;
        }

        public Player SenderPlayer
        {
            get => Main.player[SenderId];
            set => SenderId = (byte)value.whoAmI;
        }

        protected override void HandleRouting(byte fromWho, bool fromServer)
        {
            if (ReceiverId == -1 || ReceiverId == Main.myPlayer)
                DoAction();
            if (fromWho == Main.myPlayer && Main.netMode == NetmodeID.MultiplayerClient || Main.netMode == NetmodeID.Server)
                Send(ReceiverId, fromWho);
        }
    }
}
