using Spellwright.Content.Buffs.Spells.Defensive;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base.SpellCosts;
using Spellwright.Content.Spells.Base.SpellCosts.Reagent;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Network.RoutedHandlers.Spell;
using Spellwright.Util;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.BuffSpells.Defensive
{
    internal class ReactiveArmorSpell : BuffSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 8;

            int buff = ModContent.BuffType<ReactiveArmorBuff>();
            AddEffect(buff, (playerLevel) => UtilTime.MinutesToTicks(10 + 3 * playerLevel));

            UnlockCost = new OptionalSpellCost()
                .WithCost(ItemID.AdamantiteBar, 20)
                .WithCost(ItemID.TitaniumBar, 20);

            CastCost = new ReagentSpellCost(ModContent.ItemType<RareSpellReagent>(), 3);
        }
        protected override void DoExtraActions(IEnumerable<Player> players, int playerLevel)
        {
            base.DoExtraActions(players, playerLevel);

            int maxBonusDefense = 4 + 2 * playerLevel;
            foreach (Player player in players)
                new ReactiveArmorSetDefenseAction(player.whoAmI, 0, maxBonusDefense).Execute();
        }
    }
}
