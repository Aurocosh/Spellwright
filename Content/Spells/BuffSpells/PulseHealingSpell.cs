using Spellwright.Content.Buffs.Spells;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using Terraria;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.BuffSpells
{
    internal class PulseHealingSpell : BuffSpell
    {
        public override void SetStaticDefaults()
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
