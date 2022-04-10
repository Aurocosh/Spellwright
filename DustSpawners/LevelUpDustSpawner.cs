using Microsoft.Xna.Framework;
using Spellwright.Extensions;
using Terraria;
using Terraria.ID;

namespace Spellwright.DustSpawners
{
    internal class LevelUpDustSpawner : DustSpawner
    {
        public Player Caster { get; set; }
        public int Level { get; set; } = 0;

        public LevelUpDustSpawner(Player caster, int level)
        {
            Caster = caster;
            Level = level;
        }

        public override void Spawn()
        {
            if (Level >= 1)
                SpawnRing(DustID.CopperCoin, 3);
            if (Level >= 2)
                SpawnRing(DustID.CopperCoin, 5);
            if (Level >= 3)
                SpawnRing(DustID.CopperCoin, 7);
            if (Level >= 4)
                SpawnRing(DustID.CopperCoin, 9);
            if (Level >= 5)
                SpawnRing(DustID.SilverCoin, 11);
            if (Level >= 6)
                SpawnRing(DustID.SilverCoin, 13);
            if (Level >= 7)
                SpawnRing(DustID.SilverCoin, 15);
            if (Level >= 8)
                SpawnRing(DustID.GoldCoin, 17);
            if (Level >= 9)
                SpawnRing(DustID.GoldCoin, 19);
            if (Level >= 10)
                SpawnRing(DustID.GoldCoin, 21);
        }

        private void SpawnRing(int dustType, int radius)
        {
            var position = Caster.Center;
            int worldRadius = radius * 16;
            int minRadius = worldRadius - 1;
            int maxRadius = worldRadius + 1;
            int dustCount = radius * radius;

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
