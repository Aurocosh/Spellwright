using Spellwright.Core.Spells;
using Terraria;

namespace Spellwright.Core.Links
{
    internal class KnownSpellsLinkHandler : ILinkHandler
    {
        public string Type => "known_spells";
        public string ProcessLink(string link, Player player)
        {
            return SpellInfoProvider.GetSpellList(player, false);
        }
    }
}
