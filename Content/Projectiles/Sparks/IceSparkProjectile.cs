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
            base.SetStaticDefaults();
            // DisplayName.SetDefault("Ice spark");
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn, UtilTime.SecondsToTicks(20));
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.Frostburn, UtilTime.SecondsToTicks(20));
        }
    }
}