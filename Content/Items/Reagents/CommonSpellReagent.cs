using Spellwright.Config;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Items.Reagents
{
    public class CommonSpellReagent : ModItem
    {
        public CommonSpellReagent()
        {
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Common Spell Reagent");
            Tooltip.SetDefault("Collection of most common spell reagents\n" +
                "required for spellcraft.");
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.ammo = Item.type;
            //Item.value = Item.buyPrice(0, 0, 0, 10);
            Item.value = SpellwrightServerConfig.Instance.CommonReagentCost;
            Item.rare = ItemRarityID.Green;
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