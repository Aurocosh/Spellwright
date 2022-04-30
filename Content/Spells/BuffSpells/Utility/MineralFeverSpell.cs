using Spellwright.Content.Buffs.Spells.Utility;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base.SpellCosts.Reagent;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.BuffSpells.Utility
{
    internal class MineralFeverSpell : BuffSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 1;

            static int durationGetter(int playerLevel) => UtilTime.MinutesToTicks(8 + 3.2f * playerLevel);
            AddEffect(ModContent.BuffType<MineralFeverBuff>(), durationGetter);

            CastCost = new ReagentSpellCost(ModContent.ItemType<CommonSpellReagent>(), 25);
        }
    }
}
