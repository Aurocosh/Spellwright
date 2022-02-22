using Terraria;
using Terraria.ModLoader;

namespace Spellwright.Content.Items.Reagents
{
    public class RareSpellReagent : ModItem
    {
        public RareSpellReagent()
        {
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rare Spell Reagent");
        }

        public override void SetDefaults()
        {
            Item.maxStack = 99;
            Item.ammo = Item.type;
        }
        public override void AddRecipes()
        {
            //CreateRecipe()
            //    .AddIngredient(ItemID.SilverBar, 1)
            //    .AddIngredient(ItemID.IronBar, 1)
            //    .Register();
        }
    }
}