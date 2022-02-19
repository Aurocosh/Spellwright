using Terraria;
using Terraria.ModLoader;

namespace Spellwright.Content.Buffs.Spells
{
    public class GreedyVortexBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Greedy Vortex");
            Description.SetDefault("Increases pick up range for all items.");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
        }
    }
}