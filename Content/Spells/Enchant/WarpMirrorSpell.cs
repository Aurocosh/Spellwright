using Spellwright.Content.Items.Mirrors;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.SpellCosts;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.Enchant
{
    internal class WarpMirrorSpell : BindMirrorSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 2;
            UseType = SpellType.Invocation;
            itemType = ModContent.ItemType<WarpedMagicMirror>();

            var itemSpellCost = new MultipleItemSpellCost();
            itemSpellCost.AddItemCost(ModContent.ItemType<SilverMirror>());
            itemSpellCost.AddItemCost(ModContent.ItemType<CommonSpellReagent>(), 2);
            spellCost = itemSpellCost;
        }
    }
}