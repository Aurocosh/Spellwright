using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.Base.Types
{
    internal abstract class MinionSummonSpell : ModSpell
    {
        protected int buffType;
        protected int projectileType;
        protected int maxSummonCount;

        protected virtual int GetProjectileType(int playerLevel) => projectileType;
        protected virtual int GetBuffType(int playerLevel) => buffType;
        protected virtual int GetMaxSummonCount(int playerLevel) => maxSummonCount;

        public MinionSummonSpell()
        {
            UseType = SpellType.Invocation;

            damage = 0;
            knockback = 0;
            canAutoReuse = false;
            useTimeMultiplier = 1f;
            useSound = SoundID.Item44;
            damageType = DamageClass.Summon;
            buffType = -1;
            projectileType = -1;
            maxSummonCount = -1;
        }
        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            int realDamage = GetDamage(playerLevel);
            float realKnockback = GetKnockback(playerLevel);
            int realBuffType = GetBuffType(playerLevel);
            int realProjectileType = GetProjectileType(playerLevel);
            int realMaxSummonCount = GetMaxSummonCount(playerLevel);

            if (realMaxSummonCount > 0)
            {
                int summonCount = player.ownedProjectileCounts[realProjectileType];
                if (summonCount >= realMaxSummonCount)
                    return false;
            }

            player.AddBuff(realBuffType, 2);

            var position = player.Center;
            position.Y -= 50;
            var projectileSource = new ProjectileSource_Item(player, null);
            var projectile = Projectile.NewProjectileDirect(projectileSource, position, Vector2.Zero, realProjectileType, realDamage, realKnockback, Main.myPlayer);
            projectile.originalDamage = realDamage;

            return true;
        }
    }
}
