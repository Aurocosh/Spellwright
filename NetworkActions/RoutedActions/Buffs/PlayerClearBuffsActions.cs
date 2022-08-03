using Spellwright.Extensions;
using Spellwright.Network.NetworkActions;
using System;

namespace Spellwright.Network.RoutedHandlers.Buffs
{
    [Serializable]
    internal class PlayerClearBuffsActions : RoutedNetworkAction
    {
        public int[] BuffIds { get; set; }

        public PlayerClearBuffsActions(int playerId, int buffId) : base(playerId)
        {
            BuffIds = new int[] { buffId };
        }

        public PlayerClearBuffsActions(int playerId, int[] buffIds) : base(playerId)
        {
            BuffIds = buffIds;
        }

        public override void DoAction()
        {
            ReceiverPlayer.ClearBuffs(BuffIds);
        }
    }
}
