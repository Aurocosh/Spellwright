using Spellwright.Common.Players;
using Spellwright.Network.NetworkActions;
using System;

namespace Spellwright.Network.Sync
{
    [Serializable]
    internal class PermanentPlayerEffectsSyncAction : SyncNetworkAction
    {
        public int[] BufIds { get; set; }

        public PermanentPlayerEffectsSyncAction(int senderId, int[] bufIds) : base(senderId)
        {
            BufIds = bufIds;
        }

        public PermanentPlayerEffectsSyncAction(int senderId, int receiverId, int[] bufIds) : base(senderId, receiverId)
        {
            BufIds = bufIds;
        }

        public override void DoAction()
        {
            var modPlayer = SenderPlayer.GetModPlayer<SpellwrightBuffPlayer>();
            modPlayer.SetPermanentBuffs(BufIds);
        }
    }
}
