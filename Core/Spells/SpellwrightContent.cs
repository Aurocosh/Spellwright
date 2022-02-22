using Spellwright.Content.Spells.Base;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace Spellwright.Core.Spells
{
    public static class SpellwrightContent
    {
        public static int SpellType<T>() where T : ModSpell => ModContent.GetInstance<T>()?.Type ?? 0;
        public static ModSpell GetSpell(int type) => SpellLoader.GetSpell(type);
        public static IReadOnlyList<ModSpell> GetAllSpells() => SpellLoader.GetAllSpells();
    }
}
