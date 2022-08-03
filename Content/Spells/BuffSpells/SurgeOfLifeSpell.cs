using Spellwright.Content.Buffs.Spells;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Content.Spells.Base.SpellCosts.Reagent;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Network.RoutedHandlers.Spell;
using Spellwright.Util;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.BuffSpells
{
    internal class SurgeOfLifeSpell : BuffSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 3;

            int buff = ModContent.BuffType<SurgeOfLifeBuff>();
            AddEffect(buff, (playerLevel) => UtilTime.MinutesToTicks(10 + 2 * playerLevel));

            costModifier = 0f;

            UnlockCost = new SingleItemSpellCost(ItemID.LifeCrystal);
            CastCost = new ReagentSpellCost(ModContent.ItemType<CommonSpellReagent>(), 20);
        }
        protected override void DoExtraActions(IEnumerable<Player> players, int playerLevel)
        {
            base.DoExtraActions(players, playerLevel);

            int regenRate = 2 + playerLevel;
            foreach (var player in players)
                new SurgeOfLifeSetRegenAction(player.whoAmI, regenRate).Execute();
        }
    }
}
