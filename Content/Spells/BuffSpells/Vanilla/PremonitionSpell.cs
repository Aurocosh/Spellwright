using Spellwright.Content.Spells.Base.Modifiers;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using Terraria.ID;

namespace Spellwright.Content.Spells.BuffSpells.Vanilla
{
    internal class PremonitionSpell : BuffSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 0;
            AddEffect(BuffID.Dangersense, (playerLevel) => UtilTime.MinutesToTicks(10 + 2 * playerLevel));
            AddApplicableModifier(ModifierConstants.EternalModifiers);
        }
    }
}
