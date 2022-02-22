using Spellwright.Content.Spells.Base;
using System;
using Terraria;
using Terraria.ID;

namespace Spellwright.Content.Spells.Healing
{
    internal class StitchWoundsSpell : ModSpell
    {
        protected override int GetDamage(int playerLevel) => damage + 10 * playerLevel;

        public override void SetStaticDefaults()
        {
            UseType = SpellType.Invocation;
            damage = 50;
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            int playerHealth = player.statLife;
            int maxPlayerHealth = player.statLifeMax2;
            int maxAllowedHealth = (int)(maxPlayerHealth * 0.35f);
            if (playerHealth > maxAllowedHealth)
                return false;

            int healValue = GetDamage(playerLevel);
            int maxAllowedHeal = maxAllowedHealth - playerHealth;
            int actualHeal = Math.Min(healValue, maxAllowedHeal);

            player.statLife += actualHeal;
            player.HealEffect(actualHeal);

            player.ClearBuff(BuffID.Bleeding);

            return true;
        }
    }
}