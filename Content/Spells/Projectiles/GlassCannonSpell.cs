using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Projectiles;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Content.Spells.Base.Types;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.Projectiles
{
    internal class GlassCannonSpell : ProjectileSpell
    {
        public override int GetGuaranteedUses(int playerLevel) => 10 + 4 * playerLevel;
        protected override int GetDamage(int playerLevel) => 100 * playerLevel;

        public override void SetStaticDefaults()
        {
            SpellLevel = 5;
            stability = .99f;

            damage = 20;
            knockback = 10;
            damageType = DamageClass.Magic;
            projectileType = ModContent.ProjectileType<GlassCannonProjectile>();
            projectileSpeed = 16;
            canAutoReuse = false;
            useTimeMultiplier = 3f;

            UnlockCost = new MultipleItemSpellCost()
                .WithCost(ItemID.Glass, 1)
                .WithCost(ItemID.Handgun, 1);

            SpellCost = new ReagentSpellCost(ModContent.ItemType<CommonSpellReagent>(), 15);
        }
    }
}
