using Spellwright.Data;
using Spellwright.Network.NetworkActions;
using System;
using Terraria;

namespace Spellwright.Network.RoutedHandlers.Buffs
{
    [Serializable]
    internal class PlayerAddBuffsAction : RoutedNetworkAction
    {
        public BuffData[] Effects { get; set; }

        public PlayerAddBuffsAction(int playerId, BuffData[] effects) : base(playerId)
        {
            Effects = effects;
        }

        public override void DoAction()
        {
            Player player = ReceiverPlayer;
            foreach (var buffData in Effects)
                player.AddBuff(buffData.Type, buffData.Duration);
        }
    }
}
