using Microsoft.Xna.Framework;
using Spellwright.Content.Projectiles;
using Spellwright.Util;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Buffs.Spells
{
    public class ManaStarfallBuff : ModBuff
    {
        private static readonly int minStarfallDelay = UtilTime.SecondsToTicks(5);
        private static readonly int maxStarfallDelay = UtilTime.SecondsToTicks(12);
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
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            StarlightRainPlayer rainPlayer = player.GetModPlayer<StarlightRainPlayer>();

            if (rainPlayer.NextStarDelay > 0)
            {
                rainPlayer.NextStarDelay--;
                return;
            }
            rainPlayer.NextStarDelay = UtilRandom.NextInt(minStarfallDelay, maxStarfallDelay);

            var center = player.Center;
            var shiftY = UtilRandom.NextFloat(spawnMinHeight, spawnMaxHeight);
            var shiftX = UtilRandom.NextFloat(-spawnRadius, spawnRadius);
            var velocity = Vector2.UnitY.RotatedByRandom(MathHelper.ToRadians(45));
            var scale = UtilRandom.NextFloat(13f, 17f);
            velocity *= scale;

            var spawnPosition = center + new Vector2(shiftX, -shiftY);

            int projectileId = ModContent.ProjectileType<ManaStarfallProjectile>();
            var projectileSource = new ProjectileSource_Item(player, null);
            int projectileID = Projectile.NewProjectile(projectileSource, spawnPosition, velocity, projectileId, 500, 10, player.whoAmI);
        }

        public class StarlightRainPlayer : ModPlayer
        {
            public int NextStarDelay { get; set; } = 0;
        }
    }
}
