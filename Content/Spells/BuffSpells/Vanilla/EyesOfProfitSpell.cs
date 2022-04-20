using Spellwright.Content.Spells.Base.Modifiers;
using Spellwright.Content.Spells.Base.SpellCosts;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using Terraria.ID;

namespace Spellwright.Content.Spells.BuffSpells.Vanilla
{
    internal class EyesOfProfitSpell : BuffSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 1;
            SpellCost = new SingleItemSpellCost(ItemID.GoldCoin, 2);
            AddEffect(BuffID.Spelunker, (playerLevel) => UtilTime.MinutesToTicks(10 + 2 * playerLevel));
            AddApplicableModifier(ModifierConstants.EternalModifiers);
        }
    }
}
