using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Items
{
    public class SilverMirror : ModItem
    {
        public SilverMirror()
        {
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Silver mirror");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.IceMirror); // Copies the defaults from the Ice Mirror.
            Item.consumable = true;
            Item.useAnimation = 60;
            Item.useTime = 60;
            Item.useTurn = true;
            Item.maxStack = 30;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SilverBar, 1)
                .AddIngredient(ItemID.IronBar, 1)
                .Register();
        }
    }
}