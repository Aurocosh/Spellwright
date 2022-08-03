using Spellwright.Common.Players;
using Spellwright.Network.NetworkActions;
using System;

namespace Spellwright.Network.Sync
{
    [Serializable]
    internal class PlayerLevelSyncAction : SyncNetworkAction
    {
        public int PlayerLevel { get; set; }

        public PlayerLevelSyncAction(int senderId, int playerLevel) : base(senderId)
        {
            PlayerLevel = playerLevel;
        }

        public PlayerLevelSyncAction(int senderId, int receiverId, int playerLevel) : base(senderId, receiverId)
        {
            PlayerLevel = playerLevel;
        }

        public override void DoAction()
        {
            var modPlayer = SenderPlayer.GetModPlayer<SpellwrightPlayer>();
            modPlayer.PlayerLevel = PlayerLevel;
        }
    }
}
