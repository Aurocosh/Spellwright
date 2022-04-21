using Microsoft.Xna.Framework;
using Spellwright.Content.Buffs.Minions;
using Spellwright.Extensions;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Minions
{
    public class SoulNibblerMinion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soul Nibbler");
            Main.projFrames[Projectile.type] = 6;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

            Main.projPet[Projectile.type] = true;

            ProjectileID.Sets.MinionSacrificable[Projectile.type] = false;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }

        public sealed override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 16;
            Projectile.alpha = 50;
            Projectile.tileCollide = false;

            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.minionSlots = 1f;
            Projectile.penetrate = -1;

            Projectile.aiStyle = 26;
            Projectile.netImportant = true;

            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 12;

            Projectile.timeLeft *= 5;

            AIType = ProjectileID.BabySlime;
        }

        public override bool? CanCutTiles() => false;
        public override bool MinionContactDamage() => true;

        public override void AI()
        {
            int buffType = ModContent.BuffType<BirdOfMidasBuff>();
            this.CheckBuffStatus(buffType);

            Projectile.width = 33;
            Projectile.height = 22;
            AIType = ProjectileID.BabySlime;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            float missingHealth = 0f;
            int currentPlayerId = -1;
            Vector2 position = Projectile.position;
            Player owner = Main.player[Projectile.owner];
            for (int i = 0; i < 255; i++)
            {
                Player player = Main.player[i];
                if (!player.active || player.dead)
                    continue;
                if ((owner.hostile || player.hostile) && owner.team != player.team)
                    continue;
                if (Math.Abs(player.position.X + player.width / 2 - position.X + Projectile.width / 2) + Math.Abs(player.position.Y + player.height / 2 - position.Y + Projectile.height / 2) > 1200f)
                    continue;
                if (player.statLifeMax2 - player.statLife < missingHealth)
                    continue;

                missingHealth = player.statLifeMax2 - player.statLife;
                currentPlayerId = i;
            }

            if (currentPlayerId != -1 && missingHealth > 0)
            {
                var source = new EntitySource_OnHit(Projectile, target);
                int projectileID = Projectile.NewProjectile(source, Projectile.position, Vector2.Zero, ProjectileID.SpiritHeal, 1, 1, Projectile.owner, currentPlayerId, Projectile.damage);
            }
        }
    }
}
