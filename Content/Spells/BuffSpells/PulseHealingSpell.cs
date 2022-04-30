using Spellwright.Content.Buffs.Spells;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Content.Spells.Base.SpellCosts.Reagent;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.BuffSpells
{
    internal class PulseHealingSpell : BuffSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 6;
            int buffId = ModContent.BuffType<PulseHealingBuff>();
            AddEffect(buffId, (playerLevel) => UtilTime.MinutesToTicks(4 + 2 * playerLevel));

            UnlockCost = new SingleItemSpellCost(ItemID.PixieDust, 60);
            CastCost = new ReagentSpellCost(ModContent.ItemType<CommonSpellReagent>(), 15);
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
