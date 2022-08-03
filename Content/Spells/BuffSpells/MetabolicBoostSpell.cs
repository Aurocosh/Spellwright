using Spellwright.Content.Buffs.Spells;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base.SpellCosts.Reagent;
using Spellwright.Content.Spells.Base.SpellCosts.Stats;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Network.RoutedHandlers.Spell;
using Spellwright.Util;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.BuffSpells
{
    internal class MetabolicBoostSpell : BuffSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 10;

            int buff = ModContent.BuffType<MetabolicBoostBuff>();
            AddEffect(buff, (playerLevel) => UtilTime.MinutesToTicks(2 + playerLevel));

            UnlockCost = new MaxHealthSpellCost(400);
            CastCost = new ReagentSpellCost(ModContent.ItemType<MythicalSpellReagent>(), 2);
        }
        protected override void DoExtraActions(IEnumerable<Player> players, int playerLevel)
        {
            base.DoExtraActions(players, playerLevel);

            int boostCount = (int)(.4f * playerLevel);
            foreach (Player player in players)
                new MetaBoostCountSetAction(player.whoAmI, boostCount).Execute();
        }
    }
}
