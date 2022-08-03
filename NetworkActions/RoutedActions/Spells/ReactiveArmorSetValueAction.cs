using Spellwright.Network.NetworkActions;
using System;
using static Spellwright.Content.Buffs.Spells.Defensive.ReactiveArmorBuff;

namespace Spellwright.Network.RoutedHandlers.Spell
{
    [Serializable]
    internal class ReactiveArmorSetDefenseAction : RoutedNetworkAction
    {
        public int BonusDefense { get; set; }
        public int MaxBonusDefense { get; set; }

        public ReactiveArmorSetDefenseAction(int receiverId, int bonusDefense, int maxBonusDefense) : base(receiverId)
        {
            BonusDefense = bonusDefense;
            MaxBonusDefense = maxBonusDefense;
        }

        public override void DoAction()
        {
            var modPlayer = ReceiverPlayer.GetModPlayer<ReactiveArmorPlayer>();
            modPlayer.BonusDefense = BonusDefense;
            modPlayer.MaxBonusDefense = MaxBonusDefense;
        }
    }
}
