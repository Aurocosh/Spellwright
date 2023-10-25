using Spellwright.Content.Buffs.Minions;
using Spellwright.Extensions;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Minions
{
    public class MagickaFairyMinion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

            Main.projPet[Projectile.type] = true;

            ProjectileID.Sets.MinionSacrificable[Projectile.type] = false;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }

        public sealed override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.tileCollide = false;

            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.minionSlots = 1f;
            Projectile.penetrate = -1;

            Projectile.aiStyle = 144;
            Projectile.netImportant = true;

            Projectile.timeLeft *= 5;
        }

        public override bool? CanCutTiles() => false;
        public override bool MinionContactDamage() => true;

        public override void AI()
        {
            int buffType = ModContent.BuffType<MagickaFairyBuff>();
            this.CheckBuffStatus(buffType);
        }
    }

    public class MagickaFairyPlayer : ModPlayer
    {
        // TODO_TEST
        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            int buffId = ModContent.BuffType<MagickaFairyBuff>();
            if (Player.HasBuff(buffId))
            {
                var healValue = Player.statLifeMax2 / 2 - Player.statLife;
                Player.statLife += healValue;
                Player.HealEffect(healValue);
                Player.ClearBuff(buffId);
                SoundEngine.PlaySound(SoundID.NPCHit5, Player.Center);
                return false;
            }

            return true;

        }

        //public override void ModifyHurt(ref Player.HurtModifiers modifiers)/* tModPorter Override ImmuneTo, FreeDodge or ConsumableDodge instead to prevent taking damage */
        //{
        //    if (Player.statLife < damage)
        //    {
        //        int buffId = ModContent.BuffType<MagickaFairyBuff>();
        //        if (Player.HasBuff(buffId))
        //        {
        //            var healValue = Player.statLifeMax2 / 2 - Player.statLife;
        //            Player.statLife += healValue;
        //            Player.HealEffect(healValue);
        //            Player.ClearBuff(buffId);
        //            SoundEngine.PlaySound(SoundID.NPCHit5, Player.Center);
        //            return false;
        //        }
        //    }

        //    return true;
        //}
    }
}
