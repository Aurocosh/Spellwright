using Spellwright.Content.Minions;
using Terraria;
using Terraria.ModLoader;

namespace Spellwright.Content.Buffs.Minions
{
    internal class MagickaFairyBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Magicka Fairy");
            // Description.SetDefault("Hey! Listn'! If you die, i will hate you!");

            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // If the minions exist reset the buff time, otherwise remove the buff from the player
            if (player.ownedProjectileCounts[ModContent.ProjectileType<MagickaFairyMinion>()] > 0)
            {
                player.buffTime[buffIndex] = 18000;
            }
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}
