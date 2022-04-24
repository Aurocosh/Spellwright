using Spellwright.Content.Spells.Base;
using Terraria;
using Terraria.ID;

namespace Spellwright.Content.Spells.Warp
{
    internal class HomeReflectionSpell : ModSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 0;
            UseType = SpellType.Invocation;
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            player.RemoveAllGrapplingHooks();

            for (int i = 0; i < 1000; i++)
                if (Main.projectile[i].active && Main.projectile[i].owner == player.whoAmI && Main.projectile[i].aiStyle == 7)
                    Main.projectile[i].Kill();

            // The actual method that moves the player back to bed/spawn.
            player.Spawn(PlayerSpawnContext.RecallFromItem);

            // Make dust 70 times for a cool effect. This dust is the dust at the destination.
            for (int d = 0; d < 70; d++)
                Dust.NewDust(player.position, player.width, player.height, DustID.MagicMirror, 0f, 0f, 150, default, 1.5f);

            return true;
        }
    }
}