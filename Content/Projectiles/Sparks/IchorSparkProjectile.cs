using Spellwright.Util;
using Terraria;
using Terraria.ID;

namespace Spellwright.Content.Projectiles.Sparks
{
    public class IchorSparkProjectile : SparkProjectile
    {
        public IchorSparkProjectile() : base(DustID.IchorTorch)
        {
        }

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Ichor spark");
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
            target.AddBuff(BuffID.Ichor, UtilTime.SecondsToTicks(20));
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            base.OnHitPlayer(target, damage, crit);
            target.AddBuff(BuffID.Ichor, UtilTime.SecondsToTicks(20));
        }
    }
}