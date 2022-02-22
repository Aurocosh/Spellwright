using Spellwright.Content.Projectiles;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Types;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.Explosive
{
    internal class ShapedChargeSpell : ProjectileSpell
    {
        public override int SpellLevel => 1;
        public override int GetGuaranteedUses(int playerLevel) => 12 + 3 * playerLevel;

        public override void SetStaticDefaults()
        {
            UseType = SpellType.Spell;

            damage = 1;
            knockback = 1f;
            damageType = DamageClass.Throwing;
            projectileType = ModContent.ProjectileType<ShapedChargeProjectile>();
            projectileSpeed = 10;
            canAutoReuse = false;
            useTimeMultiplier = 3f;
        }
    }
}
