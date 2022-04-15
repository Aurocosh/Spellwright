using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Spellwright.Common.Players
{
    public class SpellwrightVoidPlayer : ModPlayer
    {
        public List<Item> StoredItems = new();

        public override void SaveData(TagCompound tag)
        {
            var itemTags = new List<TagCompound>();
            foreach (var item in StoredItems)
                itemTags.Add(ItemIO.Save(item));
            tag.Add("StoredItems", itemTags);
        }

        public override void LoadData(TagCompound tag)
        {
            StoredItems.Clear();
            var itemTags = tag.GetList<TagCompound>("StoredItems");
            foreach (var itemTag in itemTags)
                StoredItems.Add(ItemIO.Load(itemTag));
        }
    }
}
