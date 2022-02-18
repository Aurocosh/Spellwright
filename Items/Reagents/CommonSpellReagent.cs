using Terraria;
using Terraria.ModLoader;

namespace Spellwright.Items.Reagents
{
    public class CommonSpellReagent : ModItem
    {
        public CommonSpellReagent()
        {
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Common Spell Reagent");
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
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