using Spellwright.Util;
using Terraria;
using Terraria.ID;

namespace Spellwright.Content.Projectiles.Sparks
{
    public class CurseSparkProjectile : SparkProjectile
    {
        public CurseSparkProjectile() : base(DustID.CursedTorch)
        {
        }

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            // DisplayName.SetDefault("Cursed spark");
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.CursedInferno, UtilTime.SecondsToTicks(20));
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.CursedInferno, UtilTime.SecondsToTicks(20));
        }
    }
}