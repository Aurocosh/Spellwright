using Spellwright.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Buffs.Spells
{
    public class MetabolicBoostBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Metabolic Boost");
            Description.SetDefault("Your metabolism is on fire");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.potionDelay == 0)
                return;

            var statPlayer = player.GetModPlayer<SpellwrightStatPlayer>();
            if (statPlayer.MetaBoostCount > 0)
            {
                player.potionDelay = 0;
                player.ClearBuff(BuffID.PotionSickness);
                statPlayer.MetaBoostCount--;
            }

            if (statPlayer.MetaBoostCount <= 0)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}