using Spellwright.Network;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Spellwright.Content.Buffs.Spells
{
    public class ReactiveArmorBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Reactive armor");
            Description.SetDefault("Grants additional defense that increases with each hit received.");
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

            public override void Hurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit)
            {
                //base.Hurt(pvp, quiet, damage, hitDirection, crit);
                if (Player.whoAmI != Main.myPlayer)
                    return;
                int minDamage = (int)(Player.statLifeMax2 * 0.3f);
                if (damage < minDamage)
                    return;
                if (BonusDefense >= MaxBonusDefense)
                    return;

                BonusDefense++;
            }
            public override void clientClone(ModPlayer clientClone)
            {
                var clone = clientClone as ReactiveArmorPlayer;
                clone.BonusDefense = BonusDefense;
                clone.MaxBonusDefense = MaxBonusDefense;
            }

            public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
            {
                int playerId = Player.whoAmI;
                ModNetHandler.reactiveArmorDefenseSync.Sync(toWho, playerId, playerId, BonusDefense);
                ModNetHandler.reactiveArmorMaxDefenseSync.Sync(toWho, playerId, playerId, MaxBonusDefense);
            }

            public override void SendClientChanges(ModPlayer clientPlayer)
            {
                var clone = clientPlayer as ReactiveArmorPlayer;
                if (clone.BonusDefense != BonusDefense)
                    ModNetHandler.reactiveArmorDefenseSync.Sync(Player.whoAmI, BonusDefense);
                if (clone.MaxBonusDefense != MaxBonusDefense)
                    ModNetHandler.reactiveArmorMaxDefenseSync.Sync(Player.whoAmI, MaxBonusDefense);
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