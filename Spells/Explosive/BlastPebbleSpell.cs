using Spellwright.Content.Projectiles;
using Spellwright.Spells.Base;
using Terraria.ModLoader;

namespace Spellwright.Spells.Explosive
{
    internal class BlastPebbleSpell : ProjectileSpell
    {
        public override int SpellLevel => 1;
        public override int GetGuaranteedUses(int playerLevel) => 5 + 1 * playerLevel;
        public BlastPebbleSpell(string name, string incantation) : base(name, incantation)
        {
            stability = .2f;

            damage = 1;
            knockback = 8f;
            damageType = DamageClass.Magic;
            projectileType = ModContent.ProjectileType<BlastPebbleProjectile>();
            projectileSpeed = 10;
            canAutoReuse = false;
            useTimeMultiplier = 3f;
        }
    }
}
