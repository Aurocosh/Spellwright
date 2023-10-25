using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Spellwright.Content.Buffs.Spells.Defensive
{
    public class ReactiveArmorBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Reactive armor");
            // Description.SetDefault("Grants additional defense that increases with each hit received.");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            var reactiveArmorPlayer = player.GetModPlayer<ReactiveArmorPlayer>();
            player.statDefense += 4 + reactiveArmorPlayer.BonusDefense;
        }

        public class ReactiveArmorPlayer : ModPlayer
        {
            public int BonusDefense { get; set; } = 0;
            public int MaxBonusDefense { get; set; } = 0;

            public override void OnHurt(Player.HurtInfo info)
            {
                if (Player.whoAmI == Main.myPlayer && BonusDefense < MaxBonusDefense)
                {
                    int minDamage = (int)(Player.statLifeMax2 * 0.2f);
                    if (info.Damage >= minDamage)
                        BonusDefense++;
                }
            }

            public override void SaveData(TagCompound tag)
            {
                tag.Add("BonusDefense", BonusDefense);
                tag.Add("MaxBonusDefense", BonusDefense);
            }

            public override void LoadData(TagCompound tag)
            {
                BonusDefense = tag.GetInt("BonusDefense");
                BonusDefense = tag.GetInt("MaxBonusDefense");
            }
        }
    }
}