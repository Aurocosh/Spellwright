using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Projectiles;
using Spellwright.Content.Spells.Base.SpellCosts.Reagent;
using Spellwright.Content.Spells.Base.Types;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.Projectiles
{
    internal class ShockwaveDartSpell : ProjectileSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 4;

            damage = 0;
            knockback = 0;
            damageType = DamageClass.Ranged;
            projectileType = ModContent.ProjectileType<ShockwaveDartProjectile>();
            projectileSpeed = 30;
            canAutoReuse = false;
            useTimeMultiplier = 3f;
            UseCost = new ReagentSpellCost(ModContent.ItemType<CommonSpellReagent>(), 1);
        }
    }
}
