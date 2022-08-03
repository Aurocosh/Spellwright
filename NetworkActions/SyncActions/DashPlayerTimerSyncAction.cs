using Spellwright.Common.Players;
using Spellwright.Network.NetworkActions;
using System;

namespace Spellwright.Network.Sync
{
    [Serializable]
    internal class DashPlayerTimerSyncAction : SyncNetworkAction
    {
        public int DashTimer { get; set; }

        public DashPlayerTimerSyncAction(int senderId, int dashTimer) : base(senderId)
        {
            DashTimer = dashTimer;
        }

        public DashPlayerTimerSyncAction(int senderId, int receiverId, int dashTimer) : base(senderId, receiverId)
        {
            DashTimer = dashTimer;
        }

        public override void DoAction()
        {
            var modPlayer = SenderPlayer.GetModPlayer<SpellwrightDashPlayer>();
            modPlayer.DashTimer = DashTimer;
        }
    }
}
