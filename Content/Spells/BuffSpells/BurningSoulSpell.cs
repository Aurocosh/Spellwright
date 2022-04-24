using Spellwright.Content.Buffs.Spells;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Content.Spells.Base.SpellCosts.Stats;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.BuffSpells
{
    internal class BurningSoulSpell : BuffSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 5;
            AddEffect(ModContent.BuffType<BurningSoulBuff>(), (playerLevel) => UtilTime.MinutesToTicks(3 + (int)(1.4f * playerLevel)));

            UnlockCost = new MultipleItemSpellCost()
                .WithCost(ItemID.LivingCursedFireBlock, 30)
                .WithCost(ItemID.ManaPotion, 10);
            CastCost = new MaxManaSpellCost(20);
        }
    }
}
