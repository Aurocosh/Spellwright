using Microsoft.Xna.Framework;
using Spellwright.Extensions;
using Spellwright.Network.Base.Executable;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;

namespace Spellwright.ExecutablePackets.Broadcast.DustSpawners
{
    internal class LevelUpDustSpawner : IExecutablePacket
    {
        public Player Caster { get; set; }
        public int[] Levels { get; set; }

        public LevelUpDustSpawner()
        {
        }

        public LevelUpDustSpawner(Player caster, IEnumerable<int> levels)
        {
            Caster = caster;
            Levels = levels.ToArray();
        }

        public void Execute()
        {
            foreach (var level in Levels)
                SpawnRing(level);
        }

        private void SpawnRing(int level)
        {
            switch (level)
            {
                case 1:
                    SpawnRing(DustID.CopperCoin, 3);
                    break;
                case 2:
                    SpawnRing(DustID.CopperCoin, 5);
                    break;
                case 3:
                    SpawnRing(DustID.CopperCoin, 7);
                    break;
                case 4:
                    SpawnRing(DustID.CopperCoin, 9);
                    break;
                case 5:
                    SpawnRing(DustID.SilverCoin, 11);
                    break;
                case 6:
                    SpawnRing(DustID.SilverCoin, 13);
                    break;
                case 7:
                    SpawnRing(DustID.SilverCoin, 15);
                    break;
                case 8:
                    SpawnRing(DustID.GoldCoin, 17);
                    break;
                case 9:
                    SpawnRing(DustID.GoldCoin, 19);
                    break;
                case 10:
                    SpawnRing(DustID.GoldCoin, 21);
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
                //Vector2 velocity = position.DirectionTo(dustPosition).ScaleRandom(.1f, .4f);
                Vector2 velocity = Main.rand.NextVector2Unit().ScaleRandom(.1f, .4f);

                var dust = Dust.NewDustDirect(dustPosition, 0, 0, dustType, 0f, 0f, 100, default, 2.0f);
                dust.velocity = velocity;
                dust.noLightEmittence = true;
            }
        }
    }
}
