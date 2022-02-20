using Spellwright.Content.Buffs.Spells;
using Spellwright.Spells.Base;
using Spellwright.Util;
using Terraria;
using Terraria.ModLoader;

namespace Spellwright.Spells.BuffSpells
{
    internal class PulseHealingSpell : BuffSpell
    {
        public PulseHealingSpell(string name, string incantation) : base(name, incantation)
        {
            int buffId = ModContent.BuffType<PulseHealingBuff>();
            AddEffect(buffId, (playerLevel) => UtilTime.MinutesToTicks(4 + 2 * playerLevel));
        }
        protected override void DoExtraActions(Player player, int playerLevel)
        {
            base.DoExtraActions(player, playerLevel);
            int healValue = 40 * playerLevel;
            var pulsePlayer = player.GetModPlayer<PulseHealingPlayer>();
            pulsePlayer.HealingValue = healValue;
        }
    }
}
