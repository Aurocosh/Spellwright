using Microsoft.Xna.Framework;
using Spellwright.Extensions;
using Spellwright.Network.Base.Executable;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace Spellwright.ExecutablePackets.Broadcast.DustSpawners
{
    internal class SoulDisturbanceSpawner : IExecutablePacket
    {
        private static SoundStyle SoundStyle = SoundID.PlayerHit;

        public Player Caster { get; set; }

        public SoulDisturbanceSpawner()
        {
        }

        public SoulDisturbanceSpawner(Player caster)
        {
            Caster = caster;
        }

        public void Execute()
        {
            var position = Caster.Center;
            int innerRadius = 5 * 16;
            int outerRadius = 20 * 16;

            for (int i = 0; i < 25; i++)
            {
                Vector2 dustPosition = position + Main.rand.NextVector2Unit().ScaleRandom(0, innerRadius);
                dustPosition.X -= 3;
                var dust = Dust.NewDustDirect(dustPosition, 0, 0, DustID.PurpleTorch, 0f, 0f, 100, default, 0.8f);
                dust.noLightEmittence = true;
                dust.noGravity = true;
            }

            for (int i = 0; i < 50; i++)
            {
                Vector2 dustPosition = position + Main.rand.NextVector2Unit().ScaleRandom(0, outerRadius);
                Vector2 velocity = position.DirectionTo(dustPosition).ScaleRandom(.1f, 2.5f);
                velocity = velocity.PerpendicularClockwise();
                velocity *= 1;

                var dust = Dust.NewDustDirect(dustPosition, 0, 0, DustID.PurpleTorch, 0f, 0f, 100, default, 0.8f);
                dust.velocity = velocity;
                dust.noLightEmittence = true;
            }

            SoundEngine.PlaySound(SoundStyle, position);
        }
    }
}
