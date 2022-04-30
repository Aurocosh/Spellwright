using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Content.Spells.Base.SpellCosts.Reagent;
using Spellwright.Content.Spells.Base.Types;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.Projectiles
{
    internal class FanOfFlamesSpell : ProjectileSpraySpell
    {
        public override int GetGuaranteedUses(int playerLevel) => 10 + 2 * playerLevel;
        protected override int GetDamage(int playerLevel) => 20 + 5 * playerLevel;
        protected override int GetProjectileCount(int playerLevel) => 5 + 1 * playerLevel;

        public override void SetStaticDefaults()
        {
            SpellLevel = 3;

            damage = 30;
            knockback = 10;
            damageType = DamageClass.Magic;
            projectileType = ProjectileID.ImpFireball;
            projectileSpeed = 20;
            canAutoReuse = false;
            useTimeMultiplier = 3f;

            projectileCount = 5;
            projectileSpray = 15;
            minSpeedChange = .2f;
            maxSpeedChange = .35f;

            UnlockCost = new SingleItemSpellCost(ItemID.LivingFireBlock, 35);
            CastCost = new ReagentSpellCost(ModContent.ItemType<CommonSpellReagent>(), 5);
        }
    }
}
