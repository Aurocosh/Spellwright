using Spellwright.Content.Projectiles;
using Spellwright.Spells.Base;
using Terraria.ModLoader;

namespace Spellwright.Spells.Explosive
{
    internal class DragonSpitSpell : ProjectileSpell
    {
        public override int SpellLevel => 1;
        public override int GetGuaranteedUses(int playerLevel) => 12 + 3 * playerLevel;
        public DragonSpitSpell(string name, string incantation) : base(name, incantation)
        {
            stability = .5f;

            damage = 1;
            knockback = 8f;
            damageType = DamageClass.Magic;
            projectileType = ModContent.ProjectileType<DragonSpitProjectile>();
            projectileSpeed = 10;
            canAutoReuse = false;
            useTimeMultiplier = 3f;
        }
    }
}
