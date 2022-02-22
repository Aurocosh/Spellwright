using Spellwright.Content.Spells.Base;
using Terraria;

namespace Spellwright.Content.Spells.Warp
{
    internal class HomeReflectionSpell : ModSpell
    {
        public override void SetStaticDefaults()
        {
            UseType = SpellType.Invocation;
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            //// Each frame, make some dust
            //if (Main.rand.NextBool())
            //{
            //    Dust.NewDust(player.position, player.width, player.height, 15, 0f, 0f, 150, Color.White, 1.1f); // Makes dust from the player's position and copies the hitbox of which the dust may spawn. Change these arguments if needed.
            //}

            // This sets up the itemTime correctly.

            // This code runs once halfway through the useTime of the Item. You'll notice with magic mirrors you are still holding the item for a little bit after you've teleported.

            player.RemoveAllGrapplingHooks();

            for (int p = 0; p < 1000; p++)
                if (Main.projectile[p].active && Main.projectile[p].owner == player.whoAmI && Main.projectile[p].aiStyle == 7)
                    Main.projectile[p].Kill();

            // The actual method that moves the player back to bed/spawn.
            player.Spawn(PlayerSpawnContext.RecallFromItem);

            // Make dust 70 times for a cool effect. This dust is the dust at the destination.
            for (int d = 0; d < 70; d++)
                Dust.NewDust(player.position, player.width, player.height, 15, 0f, 0f, 150, default, 1.5f);

            return true;
        }
    }
}