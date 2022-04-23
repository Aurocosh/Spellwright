using Spellwright.Extensions;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Spellwright.Common.Players
{
    public class SpellwrightStatPlayer : ModPlayer
    {
        public List<Item> PotionItems = new();
        public List<Item> StoredItems = new();
        public List<Item> ReagentItems = new();
        public int MetaBoostCount { get; set; } = 0;

        public override void PostUpdate()
        {
            if (PlayerInput.Triggers.JustReleased.QuickBuff && Player.CountBuffs() != Player.MaxBuffs)
                QuickBuffFromStorage();
        }

        private void QuickBuffFromStorage()
        {
            if (Player.cursed || Player.CCed || Player.dead)
                return;
            LegacySoundStyle legacySoundStyle = null;
            if (Player.CountBuffs() == Player.MaxBuffs)
                return;

            if (Player.CountBuffs() != Player.MaxBuffs)
            {
                foreach (var item in PotionItems)
                {
                    if (item.stack <= 0 || item.type <= ItemID.None || item.buffType <= 0 || item.DamageType == DamageClass.Summon)
                        continue;

                    int buffId = item.buffType;
                    bool canUseItem = CombinedHooks.CanUseItem(Player, item) && Player.QuickBuff_ShouldBotherUsingThisBuff(buffId);
                    if (item.mana > 0 && canUseItem && Player.CheckMana(item, -1, pay: true, blockQuickMana: true))
                    {
                        Player.manaRegenDelay = (int)Player.maxRegenDelay;
                    }
                    if (Player.whoAmI == Main.myPlayer && item.type == ItemID.Carrot && !Main.runningCollectorsEdition)
                    {
                        canUseItem = false;
                    }
                    if (buffId == BuffID.FairyBlue)
                    {
                        var rand = Main.rand.Next(3);
                        if (rand == 0)
                            buffId = BuffID.FairyBlue;
                        if (rand == 1)
                            buffId = BuffID.FairyRed;
                        if (rand == 2)
                            buffId = BuffID.FairyGreen;
                    }
                    if (!canUseItem)
                        continue;

                    ItemLoader.UseItem(item, Player);
                    legacySoundStyle = item.UseSound;
                    int buffTime = item.buffTime;
                    if (buffTime == 0)
                        buffTime = 3600;

                    Player.AddBuff(buffId, buffTime);
                    if (item.consumable && ItemLoader.ConsumeItem(item, Player))
                    {
                        item.stack--;
                        if (item.stack <= 0)
                            item.TurnToAir();
                    }
                    if (Player.CountBuffs() == Player.MaxBuffs)
                        break;
                }
            }
            if (legacySoundStyle != null)
            {
                SoundEngine.PlaySound(legacySoundStyle, Player.position);
                Recipe.FindRecipes();
            }
        }

        public override void SaveData(TagCompound tag)
        {
            tag.Add("MetaBoostCount", MetaBoostCount);
            tag.Add("PotionItems", PotionItems.Select(ItemIO.Save).ToList());
            tag.Add("StoredItems", StoredItems.Select(ItemIO.Save).ToList());
            tag.Add("ReagentItems", ReagentItems.Select(ItemIO.Save).ToList());
        }

        public override void LoadData(TagCompound tag)
        {
            MetaBoostCount = tag.GetInt("MetaBoostCount");
            PotionItems = tag.GetList<TagCompound>("PotionItems").Select(ItemIO.Load).ToList();
            StoredItems = tag.GetList<TagCompound>("StoredItems").Select(ItemIO.Load).ToList();
            ReagentItems = tag.GetList<TagCompound>("ReagentItems").Select(ItemIO.Load).ToList();
        }
    }
}
