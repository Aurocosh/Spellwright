using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Spellwright.Util
{
    internal class UtilBuff
    {
        public static bool IsModBuff(int type) => type >= BuffID.Count;

        public static TagCompound SerializeBuff(int buffId)
        {
            if (IsModBuff(buffId))
            {
                var modBuff = BuffLoader.GetBuff(buffId);
                return new TagCompound
                {
                    ["mod"] = modBuff.Mod.Name,
                    ["name"] = modBuff.Name
                };
            }
            else
            {
                return new TagCompound
                {
                    ["mod"] = "Terraria",
                    ["id"] = buffId
                };
            }
        }

        public static int DeserializeBuff(TagCompound tag)
        {
            var modName = tag.GetString("mod");
            return modName == "Terraria" ? tag.GetInt("id") : ModContent.TryFind(modName, tag.GetString("name"), out ModBuff buff) ? buff.Type : 0;
        }
    }
}
