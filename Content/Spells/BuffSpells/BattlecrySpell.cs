using Spellwright.Content.Buffs.Spells;
using Spellwright.Content.Spells.Base.Modifiers;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.BuffSpells
{
    internal class BattlecrySpell : BuffSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 2;
            AddEffect(ModContent.BuffType<BattlecryBuff>(), (playerLevel) => UtilTime.MinutesToTicks(10 + 2 * playerLevel));
            RemoveApplicableModifier(SpellModifier.IsAoe);
            RemoveApplicableModifier(SpellModifier.IsSelfless);

            AddApplicableModifier(ModifierConstants.EternalModifiers);
        }
    }
}
