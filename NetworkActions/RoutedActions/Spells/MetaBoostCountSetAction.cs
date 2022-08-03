using Spellwright.Common.Players;
using Spellwright.Network.NetworkActions;
using System;

namespace Spellwright.Network.RoutedHandlers.Spell
{
    [Serializable]
    internal class MetaBoostCountSetAction : RoutedNetworkAction
    {
        public int BoostCount { get; set; }

        public MetaBoostCountSetAction(int receiverId, int boostCount) : base(receiverId)
        {
            BoostCount = boostCount;
        }

        public override void DoAction()
        {
            var statPlayer = ReceiverPlayer.GetModPlayer<SpellwrightStatPlayer>();
            statPlayer.MetaBoostCount = BoostCount;
        }
    }
}
