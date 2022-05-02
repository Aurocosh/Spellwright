using System;
using Terraria;

namespace Spellwright.Core.Links
{
    internal class HomeLinkHandler : ILinkHandler
    {
        public string Type => "home_page";

        public string ProcessLink(string link, Player player)
        {
            string myStatus = Spellwright.GetTranslation("MenuText", "MyStatus").Value;
            string knownSpells = Spellwright.GetTranslation("MenuText", "KnownSpells").Value;
            string usableSpells = Spellwright.GetTranslation("MenuText", "UsableSpells").Value;

            var output = string.Join(
                Environment.NewLine,
                "Menu",
                $"1) [{myStatus}](link = player_status,color = #FFCC66)",
                $"2) [{knownSpells}](link = known_spells)",
                $"3) [{usableSpells}](link = usable_spells)");
            return output;
        }
    }
}
