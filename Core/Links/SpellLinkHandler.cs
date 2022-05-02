using Spellwright.Common.Players;
using Spellwright.Content.Spells.Base;
using Spellwright.Core.Spells;
using Terraria;

namespace Spellwright.Core.Links
{
    internal class SpellLinkHandler : ILinkHandler
    {
        public string Type => "spell";

        public string ProcessLink(string link, Player player)
        {
            var spell = SpellLibrary.GetSpellByName(link);
            if (spell == null)
                return null;

            var spellPlayer = player.GetModPlayer<SpellwrightPlayer>();
            return SpellInfoProvider.GetSpellDescription(player, spellPlayer.PlayerLevel, spell, SpellData.EmptyData, true, true);
        }
    }
}
