using Spellwright.Content.Buffs.Spells.Defensive;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Modifiers;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Content.Spells.Base.SpellCosts.Reagent;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Extensions;
using Spellwright.Util;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.BuffSpells.Defensive
{
    internal class FortressStanceSpell : BuffSpell
    {
        public override int GetUseDelay(int playerLevel)
        {
            //return UtilTime.SecondsToTicks(1);
            return UtilTime.SecondsToTicks(20 - (int)(0.8f * playerLevel));
        }

        protected override void DoExtraActions(IEnumerable<Player> affectedPlayers, int playerLevel)
        {
            int radius = 2 * 16;
            var perimeter = 2 * Math.PI * radius;
            int dustCount = (int)(perimeter / 4);
            foreach (Player player in affectedPlayers)
                for (int i = 0; i < dustCount; i++)
                {
                    var dustPosition = player.Center + Main.rand.NextVector2Unit().ScaleRandom(radius - 1, radius + 1);
                    Dust.NewDust(dustPosition, 1, 1, DustID.Stone, 0, 0, Scale: 0.5f);
                }
        }

        public override void SetStaticDefaults()
        {
            SpellLevel = 6;
            UseType = SpellType.Cantrip;
            AddEffect(ModContent.BuffType<FortressStanceBuff>(), (playerLevel) => UtilTime.SecondsToTicks(2.5f));

            RemoveApplicableModifier(SpellModifier.Area);
            RemoveApplicableModifier(SpellModifier.Selfless);

            UnlockCost = new SingleItemSpellCost(ItemID.IronskinPotion, 60);
            CastCost = new ReagentSpellCost(ModContent.ItemType<RareSpellReagent>(), 2);
        }
    }
}
