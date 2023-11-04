using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Core.Buffs
{
    internal class BuffHandler
    {
        public static void UpdateBuff(int buffId, Player player)
        {
            int virtualBuffIndex = 0;

            switch (buffId)
            {
                case BuffID.ObsidianSkin:
                    player.lavaImmune = true;
                    player.fireWalk = true;
                    player.buffImmune[BuffID.OnFire] = true;
                    break;
                case BuffID.Featherfall:
                    player.slowFall = true;
                    break;
                case BuffID.Spelunker:
                    player.findTreasure = true;
                    break;
                case BuffID.Shine:
                    Lighting.AddLight((int)(player.position.X + player.width / 2) / 16, (int)(player.position.Y + player.height / 2) / 16, 0.8f, 0.95f, 1f);
                    break;
                case BuffID.WaterWalking:
                    player.waterWalk = true;
                    break;
                case BuffID.Dangersense:
                    player.dangerSense = true;
                    break;
                case BuffID.NightOwl:
                    player.nightVision = true;
                    break;
                case BuffID.Hunter:
                    player.detectCreature = true;
                    break;
                case BuffID.Clairvoyance:
                    player.GetCritChance(DamageClass.Magic) += 2;
                    player.GetDamage(DamageClass.Magic) += 0.05f;
                    player.statManaMax2 += 20;
                    player.manaCost -= 0.02f;
                    break;
                case BuffID.AmmoBox:
                    player.ammoBox = true;
                    break;
                case BuffID.Bewitched:
                    player.maxMinions++;
                    break;
                case BuffID.Sharpened:
                    player.GetArmorPenetration(DamageClass.Melee) += 12;
                    break;
            }

            if (virtualBuffIndex == 0)
                BuffLoader.Update(buffId, player, ref virtualBuffIndex);
        }
    }
}
