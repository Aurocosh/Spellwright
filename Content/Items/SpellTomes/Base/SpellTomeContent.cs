using Spellwright.Content.Spells.Base;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace Spellwright.Content.Items.SpellTomes.Base
{
    public class SpellTomeContent
    {
        public readonly List<ModSpell> Spells = new();

        public void AddSpell<T>()
            where T : ModSpell
        {
            var modSpell = ModContent.GetInstance<T>();
            Spells.Add(modSpell);
        }
    }
}
