using Spellwright.Content.Buffs.Spells.Utility;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Content.Spells.Base.SpellCosts.Reagent;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.BuffSpells.Utility
{
    internal class GreedyVortexSpell : BuffSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 9;
            AddEffect(ModContent.BuffType<GreedyVortexBuff>(), (playerLevel) => UtilTime.MinutesToTicks(3 + (int)(1.5f * playerLevel)));

            UnlockCost = new SingleItemSpellCost(ItemID.SoulofFlight, 20);
            CastCost = new ReagentSpellCost(ModContent.ItemType<MythicalSpellReagent>(), 3);
        }
    }
}
