using Spellwright.Content.Projectiles.Sparks;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using System;
using Terraria;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.Projectiles
{
    internal class SparkCasterSpell : ProjectileSpraySpell
    {
        private readonly int[] projectileTypes;
        protected override int GetDamage(int playerLevel) => damage + 2 * playerLevel;
        protected override int GetProjectileCount(int playerLevel) => projectileCount + (int)Math.Ceiling(0.6f * playerLevel);

        public SparkCasterSpell()
        {
            projectileTypes = new int[] {
                ModContent.ProjectileType<FireSparkProjectile>(),
                ModContent.ProjectileType<PoisonSparkProjectile>(),
                ModContent.ProjectileType<IceSparkProjectile>(),
                ModContent.ProjectileType<CurseSparkProjectile>(),
                ModContent.ProjectileType<IchorSparkProjectile>(),
            };
        }

        public override void SetStaticDefaults()
        {
            UseType = SpellType.Cantrip;

            useDelay = UtilTime.SecondsToTicks(2);

            damage = 1;
            knockback = 0;
            damageType = DamageClass.Magic;
            projectileType = ModContent.ProjectileType<FireSparkProjectile>();
            projectileSpeed = 14;

            projectileCount = 4;
            projectileSpray = 12;
            minSpeedChange = 0f;
            maxSpeedChange = .35f;
        }
        protected override int GetProjectileType(int playerLevel)
        {
            int maxProjAvailable = 1;
            if (playerLevel >= 8)
                maxProjAvailable = 5;
            else if (playerLevel >= 6)
                maxProjAvailable = 4;
            else if (playerLevel >= 4)
                maxProjAvailable = 3;
            else if (playerLevel >= 2)
                maxProjAvailable = 2;

            int index = Main.rand.Next(maxProjAvailable);

            return projectileTypes[index];
        }

    }
}
