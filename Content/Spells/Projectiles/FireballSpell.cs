using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base.SpellCosts.Reagent;
using Spellwright.Content.Spells.Base.Types;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.Projectiles
{
    internal class FireballSpell : ProjectileSpell
    {
        //public override int GetGuaranteedUses(int playerLevel) => 0;
        public override int GetGuaranteedUses(int playerLevel) => 0 + 5 * playerLevel;
        protected override int GetDamage(int playerLevel) => 50 + 10 * playerLevel;

        public override void SetStaticDefaults()
        {
            SpellLevel = 1;

            damage = 20;
            knockback = 5;
            damageType = DamageClass.Magic;
            projectileType = ProjectileID.ImpFireball;
            projectileSpeed = 35;
            canAutoReuse = false;
            useTimeMultiplier = 5f;

            //CastCost = new ReagentSpellCost(ModContent.ItemType<CommonSpellReagent>(), 1);

            UseCost = new ReagentSpellCost(ModContent.ItemType<CommonSpellReagent>(), 1);
        }
    }
}
