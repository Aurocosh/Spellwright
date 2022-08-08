using Spellwright.Content.Spells.Base;
using System.Collections.Generic;
using Terraria.Localization;

namespace Spellwright.Core
{
    public class SpellComparer : IComparer<ModSpell>
    {
        public int Compare(ModSpell a, ModSpell b)
        {
            var aName = a.DisplayName.GetTranslation(Language.ActiveCulture);
            var bName = b.DisplayName.GetTranslation(Language.ActiveCulture);
            return aName.CompareTo(bName);
        }
    }
}
