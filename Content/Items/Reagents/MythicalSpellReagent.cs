using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Items.Reagents
{
    public class MythicalSpellReagent : ModItem
    {
        public MythicalSpellReagent()
        {
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mythical Spell Reagent");
            Tooltip.SetDefault("Extremely rare and powerful reagents required for\ncreation of extremely powerful spells.");
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.ammo = Item.type;
            Item.value = Item.buyPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Red;
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