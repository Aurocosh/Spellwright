using Microsoft.Xna.Framework;
using Spellwright.Content.Projectiles;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Types;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.TileSpawn
{
    internal class BlockSpitterSpell : ProjectileSpell
    {
        public override int GetGuaranteedUses(int playerLevel) => 30 + 5 * playerLevel;

        public override void SetStaticDefaults()
        {
            SpellLevel = 3;
            UseType = SpellType.Spell;

            stability = .2f;

            damage = 1;
            knockback = 8f;
            damageType = DamageClass.Magic;
            projectileType = ModContent.ProjectileType<BlockSpitterProjectile>();
            projectileSpeed = 10;
            canAutoReuse = false;
            useTimeMultiplier = 9f;
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData, IEntitySource source, Vector2 position, Vector2 direction)
        {
            if (ActivateProjectiles(player))
                return false;
            if (BlockSpitterProjectile.GetValidItem(player) == null)
            {
                var error = Spellwright.GetTranslation("Spells", Name, "NoBlocksInToolbar");
                Main.NewText(error, Color.Orange);
                return false;
            }

            return base.Cast(player, playerLevel, spellData, source, position, direction);
        }

        private bool ActivateProjectiles(Player player)
        {
            int whoAmI = player.whoAmI;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                var projectile = Main.projectile[i];
                if (projectile.active && projectile.owner == whoAmI && projectile.type == projectileType)
                {
                    projectile.Kill();
                    return true;
                }
            }

            return false;
        }
    }
}
