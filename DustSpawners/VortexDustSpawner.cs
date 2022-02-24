using Microsoft.Xna.Framework;
using Spellwright.Extensions;
using Terraria;

namespace Spellwright.DustSpawners
{
    internal class VortexDustSpawner : DustSpawner
    {
        public Player Caster { get; set; }
        public int DustType { get; set; } = 0;
        public byte DustCount { get; set; } = 0;
        public byte Radius { get; set; } = 0;

        public override void Spawn()
        {
            var position = Caster.Center;
            int radius = Radius * 16;
            for (int i = 0; i < DustCount; i++)
            {
                Vector2 dustPosition = position + Main.rand.NextVector2Unit().ScaleRandom(0, radius);
                Vector2 velocity = position.DirectionTo(dustPosition).ScaleRandom(.1f, 2.5f);
                velocity = velocity.PerpendicularClockwise();
                velocity *= 1;

                var dust = Dust.NewDustDirect(dustPosition, 22, 22, DustType, 0f, 0f, 100, default, 1.5f);
                dust.velocity = velocity;
                dust.noLightEmittence = true;
            }
        }
    }
}
