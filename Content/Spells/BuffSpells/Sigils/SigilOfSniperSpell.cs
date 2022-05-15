using Spellwright.Content.Buffs.Spells.Sigils;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Content.Spells.Base.SpellCosts.Reagent;
using Spellwright.Util;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.BuffSpells.Sigils
{
    internal class SigilOfSniperSpell : SigilBuffSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 7;
            AddEffect(ModContent.BuffType<SigilOfSniperBuff>(), (playerLevel) => UtilTime.MinutesToTicks(4 + 0.5f * playerLevel));

            UnlockCost = new MultipleItemSpellCost()
                .WithCost(ItemID.RangerEmblem, 1)
                .WithCost(ItemID.SoulofMight, 15);

            CastCost = new ReagentSpellCost(ModContent.ItemType<MythicalSpellReagent>(), 1);
        }
    }
}
