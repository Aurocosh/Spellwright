using Microsoft.Xna.Framework;
using Spellwright.Content.Projectiles.Tiles;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.TileBreak
{
    internal class WallCrumblerSpell : ProjectileSpell
    {
        public override int GetGuaranteedUses(int playerLevel) => 30 + 5 * playerLevel;

        public override void SetStaticDefaults()
        {
            SpellLevel = 4;
            UseType = SpellType.Spell;

            stability = .2f;

            damage = 1;
            knockback = 8f;
            damageType = DamageClass.Magic;
            projectileType = ModContent.ProjectileType<WallCrumblerProjectile>();
            projectileSpeed = 10;
            canAutoReuse = false;
            useTimeMultiplier = 9f;
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData, IEntitySource source, Vector2 position, Vector2 direction)
        {
            if (ActivateProjectiles(player))
                return false;
            return base.Cast(player, playerLevel, spellData, source, position, direction);
        }

        private bool ActivateProjectiles(Player player)
        {
            foreach (var projectile in UtilProjectiles.FindProjectiles(player.whoAmI, projectileType))
            {
                projectile.Kill();
                return true;
            }
            return false;
        }
    }
}
