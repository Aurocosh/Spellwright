using Terraria;
using Terraria.ModLoader;

namespace Spellwright.Extensions
{
    internal static class ModProjectileExtensions
    {
        public static bool CheckBuffStatus(this ModProjectile modProjectile, int buffId)
        {
            Player owner = Main.player[modProjectile.Projectile.owner];

            if (owner.dead || !owner.active)
            {
                owner.ClearBuff(buffId);
                return false;
            }

            if (owner.HasBuff(buffId))
                modProjectile.Projectile.timeLeft = 2;

            return true;
        }
    }
}
