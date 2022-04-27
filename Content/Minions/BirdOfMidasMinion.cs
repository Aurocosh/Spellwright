using Spellwright.Content.Buffs.Minions;
using Spellwright.Extensions;
using Spellwright.Util;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Minions
{
    public class BirdOfMidasMinion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Raven of Midas");
            Main.projFrames[Projectile.type] = 8;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

            Main.projPet[Projectile.type] = true;

            ProjectileID.Sets.MinionSacrificable[Projectile.type] = false;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }

        public sealed override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.tileCollide = true;

            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.minionSlots = 0f;
            Projectile.penetrate = -1;

            Projectile.aiStyle = 54;
            Projectile.netImportant = true;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = UtilTime.SecondsToTicks(1);
        }

        public override bool? CanCutTiles() => false;
        public override bool MinionContactDamage() => true;

        public override void AI()
        {
            int buffType = ModContent.BuffType<BirdOfMidasBuff>();
            this.CheckBuffStatus(buffType);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Midas, UtilTime.SecondsToTicks(5), true);
        }
    }
}
