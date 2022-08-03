using Spellwright.Content.Buffs.Spells;
using Spellwright.Network.NetworkActions;
using System;

namespace Spellwright.Network.RoutedHandlers.Spell
{
    [Serializable]
    internal class SurgeOfLifeSetRegenAction : RoutedNetworkAction
    {
        public int RegenValue { get; set; }

        public SurgeOfLifeSetRegenAction(int receiverId, int regenValue) : base(receiverId)
        {
            RegenValue = regenValue;
        }

        public override void DoAction()
        {
            var modPlayer = ReceiverPlayer.GetModPlayer<SurgeOfLifePlayer>();
            modPlayer.LifeRegenValue = RegenValue;
        }
    }
}
