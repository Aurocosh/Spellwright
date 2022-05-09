using Microsoft.Xna.Framework;
using Spellwright.Content.Projectiles;
using Spellwright.Util;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Spellwright.Content.Buffs.Spells
{
    public class ManaStarfallBuff : ModBuff
    {
        private static int nextStarDelay = 0;
        private static readonly int minStarfallDelay = UtilTime.SecondsToTicks(1);
        private static readonly int maxStarfallDelay = UtilTime.SecondsToTicks(6);
        private static readonly int spawnMinHeight = 30 * 16;
        private static readonly int spawnMaxHeight = 60 * 16;
        private static readonly int spawnRadius = 60 * 16;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mana starfall");
            Description.SetDefault("Mana stars fall from the sky!");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.whoAmI != Main.myPlayer)
                return;

            if (nextStarDelay > 0)
            {
                nextStarDelay--;
                return;
            }
            nextStarDelay = Main.rand.Next(minStarfallDelay, maxStarfallDelay);

            var center = player.Center;
            var shiftY = Main.rand.NextFloat(spawnMinHeight, spawnMaxHeight);
            var shiftX = Main.rand.NextFloat(-spawnRadius, spawnRadius);
            var velocity = Vector2.UnitY.RotatedByRandom(MathHelper.ToRadians(45));
            var scale = Main.rand.NextFloat(13f, 17f);
            velocity *= scale;

            var spawnPosition = center + new Vector2(shiftX, -shiftY);

            int projectileId = ModContent.ProjectileType<ManaStarfallProjectile>();
            var projectileSource = new EntitySource_Parent(player);
            Projectile.NewProjectile(projectileSource, spawnPosition, velocity, projectileId, 500, 10, player.whoAmI);
        }
    }
}
