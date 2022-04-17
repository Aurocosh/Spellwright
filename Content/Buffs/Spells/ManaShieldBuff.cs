using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Spellwright.Content.Buffs.Spells
{
    public class ManaShieldBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mana Shield");
            Description.SetDefault("Thick layer of mana will soften the blow");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            var modPlayer = player.GetModPlayer<ManaShieldPlayer>();
            modPlayer.hasManaShield = true;
        }

        public class ManaShieldPlayer : ModPlayer
        {
            public bool hasManaShield;

            public override void ResetEffects()
            {
                hasManaShield = false;
            }

            public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
            {
                if (hasManaShield)
                {
                    int maxBlockedDamage = (int)(damage * .3f);
                    int blockedDamage = Math.Min(maxBlockedDamage, Player.statMana);
                    Player.statMana -= blockedDamage;
                    damage -= blockedDamage;
                }
                return true;
            }
        }
    }
}