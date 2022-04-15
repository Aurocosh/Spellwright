using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;

namespace Spellwright.Util
{
    internal class UtilNpc
    {
        public static IEnumerable<NPC> GetNpcInRadius(Vector2 position, int radius)
        {
            int worldRadius = radius * 16;
            long radiusSq = worldRadius * worldRadius;
            foreach (NPC npc in Main.npc)
            {
                float distanceSq = Vector2.DistanceSquared(npc.Center, position);
                if (distanceSq <= radiusSq)
                    yield return npc;
            }
        }
    }
}
