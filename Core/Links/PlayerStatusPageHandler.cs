using Spellwright.Common.Players;
using Spellwright.UI.Components.TextBox.Text;
using System.Collections.Generic;
using Terraria;

namespace Spellwright.Core.Links
{
    internal class PlayerStatusPageHandler : PageHandler
    {
        public PlayerStatusPageHandler()
        {
        }

        public override string ProcessLink(ref LinkData linkData, Player player)
        {
            var parts = new List<string>();

            var modPlayer = player.GetModPlayer<SpellwrightPlayer>();
            var message = Spellwright.GetTranslation("PlayerStatus", "MyCurrentLevel").Format(modPlayer.PlayerLevel);
            parts.Add(message);

            var buffPlayer = player.GetModPlayer<SpellwrightBuffPlayer>();
            if (buffPlayer.PermamentBuffs.Count == 0)
            {
                parts.Add(Spellwright.GetTranslation("PlayerStatus", "NoPermamentBuffs").Value);
            }
            else
            {
                var buffLines = new List<string>();
                buffLines.Add(Spellwright.GetTranslation("PlayerStatus", "HavePermamentBuffs").Value);
                foreach (int buffId in buffPlayer.PermamentBuffs)
                {
                    var buffName = Lang.GetBuffName(buffId);
                    var buffDescription = Lang.GetBuffDescription(buffId);
                    var line = $"{buffName} - {buffDescription}";
                    buffLines.Add(line);
                }

                parts.Add(string.Join("\n", buffLines));
            }

            return string.Join("\n\n", parts);
        }
    }
}
