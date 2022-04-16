using Spellwright.Extensions;
using Spellwright.Network.Base;
using System;
using Terraria;
using Terraria.ID;

namespace Spellwright.DustSpawners
{
    internal class HerbAoeDustSpawner : IExecutablePacket
    {
        public Player Caster { get; set; }
        public int Radius { get; set; } = 0;

        public HerbAoeDustSpawner(Player caster, int radius)
        {
            Caster = caster;
            Radius = radius;
        }

        public void Execute()
        {
            var perimeter = 2 * Math.PI * Radius * 16;
            int dustCount = (int)(perimeter / 8);
            float minRadius = Radius * 16 - 3;
            float maxRadius = Radius * 16 + 3;

            for (int i = 0; i < dustCount; i++)
            {
                var dustPosition = Caster.Center + Main.rand.NextVector2Unit().ScaleRandom(minRadius, maxRadius);
                Dust.NewDust(dustPosition, 4, 4, DustID.Clentaminator_Green, 0, 0, Scale: .5f);
            }
        }
    }
}
