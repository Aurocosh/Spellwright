using Spellwright.Network.NetworkActions;
using System;
using Terraria;

namespace Spellwright.Network.RoutedHandlers
{
    [Serializable]
    internal class PlayerHealAction : RoutedNetworkAction
    {
        public int HealValue { get; set; }
        public PlayerHealAction() : base()
        {
            HealValue = 0;
        }
        public PlayerHealAction(int receiver, int healValue) : base(receiver)
        {
            HealValue = healValue;
        }

        public override void DoAction()
        {
            Player player = ReceiverPlayer;
            player.statLife += HealValue;
            player.HealEffect(HealValue);
        }
    }
}
