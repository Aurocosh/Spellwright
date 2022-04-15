using Terraria;
using Terraria.ModLoader;

namespace Spellwright.Content.Buffs.Spells
{
    public class CallOfTheDepthsBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Call of the Depths");
            Description.SetDefault("Core of the world calls for you.");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            float surfaceY = (float)(Main.worldSurface * 16f);
            if (player.position.Y > surfaceY)
            {
                float depth = player.position.Y - surfaceY;
                float maxDepth = Main.bottomWorld - surfaceY;

                float proportion = depth / maxDepth;
                if (proportion < .2f)
                    proportion = .2f;
                else if (proportion > .9f)
                    proportion = 1f;
                float maxBonus = .5f;
                player.pickSpeed -= maxBonus * proportion;
            }
        }
    }
}