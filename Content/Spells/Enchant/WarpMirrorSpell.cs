using Spellwright.Content.Items.Mirrors;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Content.Spells.Base.SpellCosts.Reagent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.Enchant
{
    internal class WarpMirrorSpell : BindMirrorSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 3;
            UseType = SpellType.Invocation;
            itemType = ModContent.ItemType<WarpedMagicMirror>();

            UnlockCost = new MultipleItemSpellCost()
                .WithCost(ModContent.ItemType<SilverMirror>(), 1)
                .WithCost(ItemID.TeleportationPotion, 1);

            CastCost = new MultipleReagentSpellCost()
                .WithCost(ModContent.ItemType<SilverMirror>(), 1)
                .WithCost(ModContent.ItemType<CommonSpellReagent>(), 20);
        }
    }
}