using Spellwright.Content.Projectiles;
using Spellwright.Spells.Base;
using Terraria.ModLoader;

namespace Spellwright.Spells.Explosive
{
    internal class ShapedChargeSpell : ProjectileSpell
    {
        public override int SpellLevel => 1;
        public override int GetGuaranteedUses(int playerLevel) => 12 + 3 * playerLevel;
        public ShapedChargeSpell(string name, string incantation) : base(name, incantation)
        {
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
