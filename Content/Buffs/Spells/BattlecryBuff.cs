using Terraria;
using Terraria.ModLoader;

namespace Spellwright.Content.Buffs.Spells
{
    public class BattlecryBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Battlecry");
            Description.SetDefault("Brings you closer to the battle");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
        }
    }

    internal class BattlecryGlobalNPC : GlobalNPC
    {
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            if (player.HasBuff(ModContent.BuffType<BattlecryBuff>()))
            {
                spawnRate = (int)(spawnRate * 0.25f);
                maxSpawns = (int)(maxSpawns * 4f);
            }
        }
    }
}