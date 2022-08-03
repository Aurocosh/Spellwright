using Spellwright.Common.Players;
using Spellwright.Network.NetworkActions;
using System;

namespace Spellwright.Network.RoutedHandlers.PermanentEffects
{
    [Serializable]
    internal class PlayerRemovePermanentEffectAction : RoutedNetworkAction
    {
        public int[] BuffIds { get; set; }

        public PlayerRemovePermanentEffectAction(int playerId, int[] buffIds) : base(playerId)
        {
            BuffIds = buffIds;
        }

        public override void DoAction()
        {
            var modPlayer = ReceiverPlayer.GetModPlayer<SpellwrightBuffPlayer>();
            foreach (var effectId in BuffIds)
                modPlayer.PermanentBuffs.Remove(effectId);
        }
    }
}
