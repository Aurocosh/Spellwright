using Spellwright.Core.Spells;
using Spellwright.UI.Components.TextBox.Text;
using Terraria;

namespace Spellwright.Core.Links
{
    internal class KnownSpellsPageHandler : PageHandler
    {
        public KnownSpellsPageHandler()
        {
        }

        public override string ProcessLink(ref LinkData linkData, Player player)
        {
            return SpellInfoProvider.GetSpellList(player, false);
        }
    }
}
