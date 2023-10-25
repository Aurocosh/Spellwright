using Spellwright.Config;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Items.Reagents
{
    public class RareSpellReagent : ModItem
    {
        public RareSpellReagent()
        {
        }

        public override void SetDefaults()
        {
            Item.maxStack = 9999;
            Item.ammo = Item.type;
            //Item.value = Item.buyPrice(0, 0, 10, 0);
            Item.value = SpellwrightServerConfig.Instance.RareReagentCost;
            Item.rare = ItemRarityID.LightPurple;
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