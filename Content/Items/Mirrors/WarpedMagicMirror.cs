using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace Spellwright.Content.Items.Mirrors
{
    public class WarpedMagicMirror : BoundMagicMirror
    {
        public WarpedMagicMirror()
        {
        }

        protected override Color ParticleColor => Color.Green;

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.IceMirror); // Copies the defaults from the Ice Mirror.
            Item.consumable = true;
            Item.useAnimation = 60;
            Item.useTime = 60;
            Item.useTurn = true;

            Item.value = Item.buyPrice(0, 0, 6, 0);
            Item.rare = ItemRarityID.Green;
        }
    }
}