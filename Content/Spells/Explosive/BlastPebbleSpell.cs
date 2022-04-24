using Spellwright.Content.Projectiles.Explosive;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Modifiers;
using Spellwright.Content.Spells.Base.Types;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.Explosive
{
    internal class BlastPebbleSpell : ProjectileSpell
    {
        public BlastPebbleSpell()
        {
            AddApplicableModifier(ModifierConstants.UsebleModifiers);
        }

        public override int GetGuaranteedUses(int playerLevel) => 5 + 1 * playerLevel;

        public override void SetStaticDefaults()
        {
            SpellLevel = 4;
            UseType = SpellType.Spell;

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
