using Spellwright.Content.Spells.TileSpawn;
using Spellwright.Util;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Spellwright.Common.Players
{
    public class SpellwrightStatPlayer : ModPlayer
    {
        public bool PotionsLocked = false;
        public bool ReagentsLocked = false;
        public List<Item> PotionItems = new();
        public List<Item> StoredItems = new();
        public List<Item> ReagentItems = new();
        public int MetaBoostCount { get; set; } = 0;
        public bool AutoTorches { get; set; } = false;

        public override void PostUpdate()
        {
            if (PlayerInput.Triggers.JustReleased.QuickBuff && Player.CountBuffs() != Player.MaxBuffs && !PotionsLocked)
                UtilPlayer.QuickBuffFromStorage(Player, PotionItems);

            if (AutoTorches && Player.UsingBiomeTorches)
                WillOfTorchGodSpell.AutoPlaceTorches(Player);
        }

        public override void SaveData(TagCompound tag)
        {
            tag.Add("MetaBoostCount", MetaBoostCount);
            tag.Add("AutoTorches", AutoTorches);
            tag.Add("PotionsLocked", PotionsLocked);
            tag.Add("ReagentsLocked", ReagentsLocked);
            tag.Add("PotionItems", PotionItems.Select(ItemIO.Save).ToList());
            tag.Add("StoredItems", StoredItems.Select(ItemIO.Save).ToList());
            tag.Add("ReagentItems", ReagentItems.Select(ItemIO.Save).ToList());
        }

        public override void LoadData(TagCompound tag)
        {
            MetaBoostCount = tag.GetInt("MetaBoostCount");
            AutoTorches = tag.GetBool("AutoTorches");
            PotionsLocked = tag.GetBool("PotionsLocked");
            ReagentsLocked = tag.GetBool("ReagentsLocked");
            PotionItems = tag.GetList<TagCompound>("PotionItems").Select(ItemIO.Load).ToList();
            StoredItems = tag.GetList<TagCompound>("StoredItems").Select(ItemIO.Load).ToList();
            ReagentItems = tag.GetList<TagCompound>("ReagentItems").Select(ItemIO.Load).ToList();
        }
    }
}
