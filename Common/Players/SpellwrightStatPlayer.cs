using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Spellwright.Common.Players
{
    public class SpellwrightStatPlayer : ModPlayer
    {
        public List<Item> StoredItems = new();
        public List<Item> ReagentItems = new();
        public int MetaBoostCount { get; set; } = 0;

        public override void SaveData(TagCompound tag)
        {
            tag.Add("StoredItems", StoredItems.Select(ItemIO.Save).ToList());
            tag.Add("ReagentItems", ReagentItems.Select(ItemIO.Save).ToList());
        }

        public override void LoadData(TagCompound tag)
        {
            StoredItems = tag.GetList<TagCompound>("StoredItems").Select(ItemIO.Load).ToList();
            ReagentItems = tag.GetList<TagCompound>("ReagentItems").Select(ItemIO.Load).ToList();
        }
    }
}
