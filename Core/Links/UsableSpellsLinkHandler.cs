using Spellwright.Core.Spells;
using Terraria;

namespace Spellwright.Core.Links
{
    internal class UsableSpellsLinkHandler : ILinkHandler
    {
        public string Type => "usable_spells";
        public string ProcessLink(string link, Player player)
        {
            return SpellInfoProvider.GetSpellList(player, true);
        }
    }
}
