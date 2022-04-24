using Spellwright.Content.Buffs.Spells;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.BuffSpells
{
    internal class SelfDefenseHexSpell : BuffSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 6;
            AddEffect(ModContent.BuffType<SelfDefenseHexBuff>(), (playerLevel) => UtilTime.MinutesToTicks(4 + playerLevel));

            UnlockCost = new MultipleItemSpellCost()
                .WithCost(ItemID.SoulofNight, 20)
                .WithCost(ItemID.GuideVoodooDoll, 5);

            CastCost = new ReagentSpellCost(ModContent.ItemType<RareSpellReagent>(), 3);
        }
    }
}
