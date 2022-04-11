using Spellwright.Content.Buffs.Spells;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.BuffSpells
{
    internal class PulseHealingSpell : BuffSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 5;
            int buffId = ModContent.BuffType<PulseHealingBuff>();
            AddEffect(buffId, (playerLevel) => UtilTime.MinutesToTicks(4 + 2 * playerLevel));
        }
        protected override void DoExtraActions(IEnumerable<Player> players, int playerLevel)
        {
            base.DoExtraActions(players, playerLevel);

            int healValue = 40 * playerLevel;
            foreach (var player in players)
            {
                var pulsePlayer = player.GetModPlayer<PulseHealingPlayer>();
                pulsePlayer.HealingValue = healValue;
            }
        }
    }
}
