using Terraria;
using Terraria.ModLoader;

namespace Spellwright.Content.Buffs.Spells.Utility
{
    public class MineralFeverBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Mineral Fever");
            // Description.SetDefault("Lust for riches boils your blood.");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.pickSpeed -= .3f;

            player.GetDamage(DamageClass.Generic) *= .7f;
            player.GetDamage(DamageClass.Magic) *= .7f;
            player.GetDamage(DamageClass.Melee) *= .7f;
            player.GetDamage(DamageClass.Ranged) *= .7f;
            player.GetDamage(DamageClass.Summon) *= .7f;
            player.GetDamage(DamageClass.Throwing) *= .7f;
        }
    }
}