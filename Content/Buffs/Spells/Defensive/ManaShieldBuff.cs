using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Spellwright.Content.Buffs.Spells.Defensive
{
    public class ManaShieldBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Mana Shield");
            // Description.SetDefault("Thick layer of mana will soften the blow");
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
            private static float damageCoeff = .3f;

            public override void ResetEffects()
            {
                hasManaShield = false;
            }

            // TODO mana shield effect changed. Update description of the effect in the localization.
            public override void ModifyHurt(ref Player.HurtModifiers modifiers)
            {
                if (!hasManaShield)
                    return;

                int minimalMana = (int)Math.Floor(Player.statManaMax2 * damageCoeff);
                if (Player.statMana > minimalMana)
                    modifiers.FinalDamage *= damageCoeff;
            }

            public override void OnHurt(Player.HurtInfo info)
            {
                if (!hasManaShield)
                    return;

                int manaSpent = (int)(info.Damage * damageCoeff);
                int blockedDamage = Math.Min(manaSpent, Player.statMana);
                Player.statMana -= blockedDamage;
            }
        }
    }
}