using Microsoft.Xna.Framework;
using Spellwright.Content.Projectiles;
using Spellwright.Spells.Base;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Spells
{
    internal class StoneBulletSpell : ProjectileSpell
    {
        protected override int GetDamage(int playerLevel) => 15 + 5 * playerLevel;
        public override float GetUseSpeedMultiplier(int playerLevel) => 5f + 0.5f * playerLevel;
        public StoneBulletSpell(string name, string incantation) : base(name, incantation)
        {
            stability = 1.0f;
            knockback = 2;
            damageType = DamageClass.Throwing;
            projectileType = ModContent.ProjectileType<StoneBulletProjectile>();
            projectileSpeed = 450;
            canAutoReuse = true;
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData, IProjectileSource source, Vector2 position, Vector2 direction)
        {
            if (!player.ConsumeItem(ItemID.StoneBlock))
            {
                string message = Spellwright.GetTranslation("Messages", "RanOutOfStones");
                Main.NewText(message, new Color(255, 140, 40, 255));
                return false;
            }

            return base.Cast(player, playerLevel, spellData, source, position, direction);
        }
    }
}
