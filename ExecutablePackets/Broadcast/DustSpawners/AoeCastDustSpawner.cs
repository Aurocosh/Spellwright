using Microsoft.Xna.Framework;
using Spellwright.Extensions;
using Spellwright.Network.Base;
using Terraria;

namespace Spellwright.DustSpawners
{
    internal class AoeCastDustSpawner : IExecutablePacket
    {
        public Player Caster { get; set; }
        public Player[] AffectedPlayers { get; set; }
        public int DustType { get; set; } = 0;
        public byte EffectRadius { get; set; } = 0;
        public byte RingDustCount { get; set; } = 0;
        public byte EffectDustCount { get; set; } = 0;

        public void Execute()
        {
            SpawnAoeRing();

            foreach (var player in AffectedPlayers)
            {
                Vector2 position = player.Center;
                SpawnPlayerEffect(position);
            }
        }

        private void SpawnAoeRing()
        {
            var position = Caster.Center;
            int radius = EffectRadius * 16;
            int minRadius = radius - 5;
            int maxRadius = radius + 5;
            for (int i = 0; i < EffectDustCount; i++)
            {
                Vector2 dustPosition = position + Main.rand.NextVector2Unit().ScaleRandom(minRadius, maxRadius);
                Vector2 velocity = position.DirectionTo(dustPosition).ScaleRandom(.1f, .4f);

                var dust = Dust.NewDustDirect(dustPosition, 0, 0, DustType, 0f, 0f, 100, default, 1.0f);
                dust.velocity = velocity;
                dust.noLightEmittence = true;
            }
        }

        private void SpawnPlayerEffect(Vector2 position)
        {
            int halfOfDust = EffectDustCount / 2;
            int restOfDust = EffectDustCount - halfOfDust;

            for (int i = 0; i < halfOfDust; i++)
            {
                Vector2 dustPosition = position + Main.rand.NextVector2Unit().ScaleRandom(10, 40);
                Vector2 velocity = position.DirectionTo(dustPosition).ScaleRandom(.1f, 1.5f);

                var dust = Dust.NewDustDirect(dustPosition, 22, 22, DustType, 0f, 0f, 100, default, 1.3f);
                dust.velocity = velocity;
                dust.noLightEmittence = true;
            }

            for (int i = 0; i < restOfDust; i++)
            {
                Vector2 dustPosition = position + Main.rand.NextVector2Unit().ScaleRandom(10, 40);
                Vector2 velocity = position.DirectionTo(dustPosition).ScaleRandom(.1f, .5f);

                var dust = Dust.NewDustDirect(dustPosition, 22, 22, DustType, 0f, 0f, 100, default, 1.3f);
                dust.velocity = velocity;
                dust.noLightEmittence = true;
            }
        }
    }
}
