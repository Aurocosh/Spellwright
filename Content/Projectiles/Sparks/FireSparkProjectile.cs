using Spellwright.Util;
using Terraria;
using Terraria.ID;

namespace Spellwright.Content.Projectiles.Sparks
{
    public class FireSparkProjectile : SparkProjectile
    {
        public FireSparkProjectile() : base(DustID.Torch)
        {
        }

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Fire spark");
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
            target.AddBuff(BuffID.OnFire, UtilTime.SecondsToTicks(20));
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            base.OnHitPlayer(target, damage, crit);
            target.AddBuff(BuffID.OnFire, UtilTime.SecondsToTicks(20));
        }
    }
}