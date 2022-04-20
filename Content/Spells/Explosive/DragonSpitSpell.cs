using Spellwright.Content.Projectiles.Explosive;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Modifiers;
using Spellwright.Content.Spells.Base.SpellCosts;
using Spellwright.Content.Spells.Base.Types;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.Explosive
{
    internal class DragonSpitSpell : ProjectileSpell
    {
        public DragonSpitSpell()
        {
            AddApplicableModifier(ModifierConstants.UsebleModifiers);
        }

        public override int GetGuaranteedUses(int playerLevel) => 12 + 3 * playerLevel;

        public override void SetStaticDefaults()
        {
            SpellLevel = 1;
            UseType = SpellType.Spell;

            stability = .5f;

            damage = 1;
            knockback = 8f;
            damageType = DamageClass.Magic;
            projectileType = ModContent.ProjectileType<DragonSpitProjectile>();
            projectileSpeed = 10;
            canAutoReuse = false;
            useTimeMultiplier = 3f;

            UnlockCost = new SingleItemSpellCost(ItemID.Bomb, 20);
        }
    }
}
