using Spellwright.Util;
using Terraria;
using Terraria.ID;

namespace Spellwright.Content.Projectiles.Sparks
{
    public class IceSparkProjectile : SparkProjectile
    {
        public IceSparkProjectile() : base(DustID.IceTorch)
        {
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ice spark");
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
            target.AddBuff(BuffID.Frostburn, UtilTime.SecondsToTicks(20));
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            base.OnHitPlayer(target, damage, crit);
            target.AddBuff(BuffID.Frostburn, UtilTime.SecondsToTicks(20));
        }
    }
}