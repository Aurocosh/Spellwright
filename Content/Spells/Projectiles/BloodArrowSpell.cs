﻿using Microsoft.Xna.Framework;
using Spellwright.Content.Projectiles;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Types;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.Projectiles
{
    internal class BloodArrowSpell : ProjectileSpell
    {
        public override int GetGuaranteedUses(int playerLevel) => 25 + 5 * playerLevel;
        protected override int GetDamage(int playerLevel)
        {
            Player player = Main.LocalPlayer;
            int playerHealth = player.statLife;
            int maxPlayerHealth = player.statLifeMax2;

            int halfOfMaxHealth = (int)(maxPlayerHealth / 2f);
            float damagePercent = 0;
            if (playerHealth < halfOfMaxHealth)
                damagePercent = 1f;
            else
            {
                float overHealth = playerHealth - halfOfMaxHealth;

                float healthPercent = overHealth / halfOfMaxHealth;
                float damageBasePercent = 1 - healthPercent;
                damagePercent = damageBasePercent * damageBasePercent;
                if (damagePercent < .2f)
                    damagePercent = .2f;
            }

            float maxDamage = damage + damage * (playerLevel / 4);
            return (int)(maxDamage * damagePercent);
        }

        public override void SetStaticDefaults()
        {
            stability = .98f;

            damage = 100;
            knockback = 5;
            damageType = DamageClass.Ranged;
            projectileType = ModContent.ProjectileType<BloodArrowProjectile>();
            projectileSpeed = 30;
            canAutoReuse = false;
            useTimeMultiplier = 3f;
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData, IProjectileSource source, Vector2 position, Vector2 velocity)
        {
            base.Cast(player, playerLevel, spellData, source, position, velocity);
            int damage = (int)(player.statLifeMax2 * .08f);
            damage = (int)(player.statLifeMax2 * .36f);
            player.Hurt(PlayerDeathReason.ByCustomReason("Bleed out"), damage, 0, false, true);
            return true;
        }
    }
}