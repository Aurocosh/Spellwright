using Microsoft.Xna.Framework;
using Spellwright.Extensions;
using Spellwright.Network.Base.Executable;
using System;
using Terraria;
using Terraria.ID;

namespace Spellwright.ExecutablePackets.Broadcast.DustSpawners
{
    internal class CastDustSpawner : IExecutablePacket
    {
        public Player Caster { get; set; }
        public int Level { get; set; }

        public CastDustSpawner()
        {
        }

        public CastDustSpawner(Player caster, int level)
        {
            Caster = caster;
            Level = level;
        }

        public void Execute()
        {
            SpawnRing(Level);
        }

        private void SpawnRing(int level)
        {
            switch (level)
            {
                case <= 4:
                    SpawnRing(DustID.CopperCoin, 3);
                    break;
                case <= 7:
                    SpawnRing(DustID.SilverCoin, 3);
                    break;
                case <= 10:
                    SpawnRing(DustID.GoldCoin, 3);
                    break;
            }
        }

        private void SpawnRing(int dustType, int radius)
        {
            var position = Caster.Center;
            int worldRadius = radius * 16;
            int minRadius = worldRadius - 1;
            int maxRadius = worldRadius + 1;

            var perimeter = 2 * Math.PI * worldRadius;
            int dustCount = (int)(perimeter / 32);
            for (int i = 0; i < dustCount; i++)
            {
                Vector2 dustPosition = position + Main.rand.NextVector2Unit().ScaleRandom(minRadius, maxRadius);
                Vector2 velocity = Main.rand.NextVector2Unit().ScaleRandom(.1f, .4f);

                var dust = Dust.NewDustDirect(dustPosition, 0, 0, dustType, 0f, 0f, 100, default, 2.0f);
                dust.velocity = velocity;
                dust.noLightEmittence = true;
            }
        }
    }
}
