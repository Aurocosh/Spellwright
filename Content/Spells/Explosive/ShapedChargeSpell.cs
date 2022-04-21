using Spellwright.Content.Projectiles.Explosive;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Modifiers;
using Spellwright.Content.Spells.Base.Types;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.Explosive
{
    internal class ShapedChargeSpell : ProjectileSpell
    {
        public ShapedChargeSpell()
        {
            AddApplicableModifier(ModifierConstants.UsebleModifiers);
        }

        public override int GetGuaranteedUses(int playerLevel) => 12 + 3 * playerLevel;

        public override void SetStaticDefaults()
        {
            SpellLevel = 6;
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
