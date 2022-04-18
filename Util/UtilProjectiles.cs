using System.Collections.Generic;
using Terraria;

namespace Spellwright.Util
{
    internal class UtilProjectiles
    {
        public static IEnumerable<Projectile> FindProjectiles(int owner, int projectileType)
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                var projectile = Main.projectile[i];
                if (projectile.active && projectile.owner == owner && projectile.type == projectileType)
                    yield return projectile;
            }
        }
    }
}
