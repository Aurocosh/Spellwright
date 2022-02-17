using Spellwright.Content.Projectiles;
using Spellwright.Spells.Base;
using Spellwright.Util;
using Terraria.ModLoader;

namespace Spellwright.Spells
{
    internal class BoltOfConfusionSpell : ProjectileSpell
    {
        protected override int GetDamage(int playerLevel) => 1 + damage * playerLevel;

        public BoltOfConfusionSpell(string name, string incantation) : base(name, incantation, SpellType.Cantrip)
        {
            damage = 4;
            knockback = 0;
            damageType = DamageClass.Magic;
            projectileType = ModContent.ProjectileType<BoltOfConfusionProjectile>();
            projectileSpeed = 30;
            useDelay = UtilTime.SecondsToTicks(2);
        }
    }
}
