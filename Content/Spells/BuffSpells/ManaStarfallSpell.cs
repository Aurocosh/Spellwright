using Spellwright.Content.Buffs.Spells;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Content.Spells.Base.SpellCosts.Stats;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.BuffSpells
{
    internal class ManaStarfallSpell : BuffSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 3;
            AddEffect(ModContent.BuffType<ManaStarfallBuff>(), (playerLevel) => UtilTime.MinutesToTicks(10 + playerLevel));

            UnlockCost = new SingleItemSpellCost(ItemID.FallenStar, 50);
            CastCost = new ManaSpellCost(100);
        }
    }
}
