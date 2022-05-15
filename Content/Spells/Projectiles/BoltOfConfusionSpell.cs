using Spellwright.Content.Projectiles;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.Projectiles
{
    internal class BoltOfConfusionSpell : ProjectileSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 4;
            UseType = SpellType.Cantrip;

            damage = 30;
            knockback = 0;
            damageType = DamageClass.Magic;
            projectileType = ModContent.ProjectileType<BoltOfConfusionProjectile>();
            projectileSpeed = 30;
            useDelay = UtilTime.SecondsToTicks(2);
        }
    }
}
