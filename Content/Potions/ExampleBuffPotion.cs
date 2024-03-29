using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Potions
{
    public class ExampleBuffPotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Gives a light defense buff.");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 20;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 26;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item3;
            Item.maxStack = 30;
            Item.consumable = false;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(gold: 1);
            Item.buffType = BuffID.Ironskin; // Specify an existing buff to be applied when used.
            Item.buffTime = 5400; // The amount of time the buff declared in Item.buffType will last in ticks. 5400 / 60 is 90, so this buff will last 90 seconds.
        }

        public override void HoldStyle(Player player, Rectangle heldItemFrame)
        {
            base.HoldStyle(player, heldItemFrame);
            Item.consumable = true;
            Item.useTurn = false;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }
    }
}