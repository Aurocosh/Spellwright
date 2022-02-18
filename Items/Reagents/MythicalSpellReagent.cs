using Terraria;
using Terraria.ModLoader;

namespace Spellwright.Items.Reagents
{
    public class MythicalSpellReagent : ModItem
    {
        public MythicalSpellReagent()
        {
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mythical Spell Reagent");
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