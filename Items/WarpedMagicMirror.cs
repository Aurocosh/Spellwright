using Terraria;
using Terraria.ID;

namespace Spellwright.Items
{
    public class WarpedMagicMirror : BoundMagicMirror
    {
        public WarpedMagicMirror()
        {
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Warped Magic Mirror");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.IceMirror); // Copies the defaults from the Ice Mirror.
            Item.consumable = true;
            Item.useAnimation = 60;
            Item.useTime = 60;
            Item.useTurn = true;
            //Item.useStyle = ItemUseStyleID.HoldUp;
        }
    }
}