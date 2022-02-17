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
            DisplayName.SetDefault("Cursed spark");
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
            target.AddBuff(BuffID.CursedInferno, UtilTime.SecondsToTicks(20));
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            base.OnHitPlayer(target, damage, crit);
            target.AddBuff(BuffID.CursedInferno, UtilTime.SecondsToTicks(20));
        }
    }
}