using Spellwright.Util;
using Terraria;
using Terraria.ID;

namespace Spellwright.Content.Projectiles.Sparks
{
    public class PoisonSparkProjectile : SparkProjectile
    {
        public PoisonSparkProjectile() : base(DustID.PurpleTorch)
        {
        }

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Poisoned, UtilTime.SecondsToTicks(20));
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.Poisoned, UtilTime.SecondsToTicks(20));
        }
    }
}