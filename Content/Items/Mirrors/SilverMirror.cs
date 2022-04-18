using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Items.Mirrors
{
    public class SilverMirror : ModItem
    {
        public SilverMirror()
        {
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Silver mirror");
            Tooltip.SetDefault("Silver mirror that was polished to perfection.");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 30;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.IceMirror); // Copies the defaults from the Ice Mirror.
            Item.consumable = true;
            Item.useAnimation = 60;
            Item.useTime = 60;
            Item.useTurn = true;
            Item.maxStack = 30;
            Item.value = Item.buyPrice(0, 0, 4, 0);
            Item.rare = ItemRarityID.Green;
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