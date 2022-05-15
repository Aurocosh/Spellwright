using Spellwright.Content.Spells.TileSpawn;
using Spellwright.UI.States;
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
        public List<Item> PotionItems = new();
        public List<Item> StoredItems = new();
        public List<Item> ReagentItems = new();
        public int MetaBoostCount { get; set; } = 0;
        public bool AutoTorches { get; set; } = false;

        public override void PostUpdate()
        {
            if (PlayerInput.Triggers.JustReleased.QuickBuff && Player.CountBuffs() != Player.MaxBuffs)
            {
                var availableItems = PotionItems.Where(x => x.favorited);
                UtilPlayer.QuickBuffFromStorage(Player, availableItems);

                UIMessageState uiMessageState = Spellwright.Instance.uiMessageState;
                if (Spellwright.Instance.userInterface.CurrentState == uiMessageState)
                    uiMessageState.Refresh();
            }

            if (AutoTorches && Player.UsingBiomeTorches)
                WillOfTorchGodSpell.AutoPlaceTorches(Player);
        }

        public override void SaveData(TagCompound tag)
        {
            tag.Add("MetaBoostCount", MetaBoostCount);
            tag.Add("AutoTorches", AutoTorches);
            tag.Add("PotionItems", PotionItems.Select(ItemIO.Save).ToList());
            tag.Add("StoredItems", StoredItems.Select(ItemIO.Save).ToList());
            tag.Add("ReagentItems", ReagentItems.Select(ItemIO.Save).ToList());
        }

        public override void LoadData(TagCompound tag)
        {
            MetaBoostCount = tag.GetInt("MetaBoostCount");
            AutoTorches = tag.GetBool("AutoTorches");
            PotionItems = tag.GetList<TagCompound>("PotionItems").Select(ItemIO.Load).ToList();
            StoredItems = tag.GetList<TagCompound>("StoredItems").Select(ItemIO.Load).ToList();
            ReagentItems = tag.GetList<TagCompound>("ReagentItems").Select(ItemIO.Load).ToList();
        }
    }
}
