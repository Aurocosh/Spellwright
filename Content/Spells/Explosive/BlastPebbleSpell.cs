using Spellwright.Content.Projectiles;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Types;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.Explosive
{
    internal class BlastPebbleSpell : ProjectileSpell
    {
        public override int GetGuaranteedUses(int playerLevel) => 5 + 1 * playerLevel;

        public override void SetStaticDefaults()
        {
            SpellLevel = 3;
            UseType = SpellType.Spell;

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
