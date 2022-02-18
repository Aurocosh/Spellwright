using Spellwright.Content.Buffs;
using Spellwright.Items.Reagents;
using Spellwright.Spells.Base;
using Spellwright.Spells.SpellExtraData;
using Spellwright.Util;
using Terraria;
using Terraria.ModLoader;

namespace Spellwright.Spells.BuffSpells
{
    internal class SurgeOfLifeSpell : BuffSpell
    {
        protected override void DoExtraActions(Player player, int playerLevel)
        {
            base.DoExtraActions(player, playerLevel);
            int regenRate = 2 + playerLevel;
            var surgeOfLifePlayer = player.GetModPlayer<SurgeOfLifePlayer>();
            surgeOfLifePlayer.LifeRegenValue = regenRate;
        }

        public SurgeOfLifeSpell(string name, string incantation) : base(name, incantation)
        {
            int buff = ModContent.BuffType<SurgeOfLifeBuff>();
            AddEffect(buff, (playerLevel) => UtilTime.MinutesToTicks(10 + 2 * playerLevel));

            reagentType = ModContent.ItemType<RareSpellReagent>();
            SetExtraReagentCost(SpellModifier.IsAoe, 1);
        }
    }
}
