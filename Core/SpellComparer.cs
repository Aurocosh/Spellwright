using Spellwright.Content.Spells.Base;
using System.Collections.Generic;
using Terraria.Localization;

namespace Spellwright.Core
{
    public class SpellComparer : IComparer<ModSpell>
    {
        public int Compare(ModSpell a, ModSpell b)
        {
            //var aName = a.DisplayName.GetTranslation(Language.ActiveCulture); // TODO_TEST
            //var bName = b.DisplayName.GetTranslation(Language.ActiveCulture); // TODO_TEST
            var aName = a.DisplayName.Value; // TODO_TEST
            var bName = b.DisplayName.Value; // TODO_TEST
            return aName.CompareTo(bName);
        }
    }
}
