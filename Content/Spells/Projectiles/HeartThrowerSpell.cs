using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Projectiles;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Content.Spells.Base.SpellCosts.Stats;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.Projectiles
{
    internal class HeartThrowerSpell : ProjectileSpraySpell
    {
        protected override int GetDamage(int playerLevel) => 1;
        protected override int GetProjectileCount(int playerLevel) => 1 + (int)(0.5f * playerLevel);

        public override void SetStaticDefaults()
        {
            SpellLevel = 3;
            UseType = SpellType.Cantrip;

            damage = 4;
            knockback = 0;
            damageType = DamageClass.Magic;
            projectileType = ModContent.ProjectileType<HeartThrowerProjectile>();
            projectileSpeed = 30;
            useDelay = UtilTime.SecondsToTicks(3);
            SpellCost = new ManaSpellCost(75);

            projectileSpray = 10;
            minSpeedChange = .2f;
            maxSpeedChange = .35f;

            UnlockCost = new SingleItemSpellCost(ItemID.HeartLantern);
            SpellCost = new SingleItemSpellCost(ModContent.ItemType<RareSpellReagent>(), 5);
        }
    }
}
