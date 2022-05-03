using Spellwright.Common.Players;
using Spellwright.UI.Components.TextBox.Text;
using System.Text;
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
            var stringBuilder = new StringBuilder();

            var modPlayer = player.GetModPlayer<SpellwrightPlayer>();
            var message = Spellwright.GetTranslation("PlayerStatus", "MyCurrentLevel").Format(modPlayer.PlayerLevel);
            stringBuilder.AppendLine(message);
            stringBuilder.AppendLine();

            var buffPlayer = player.GetModPlayer<SpellwrightBuffPlayer>();
            if (buffPlayer.PermamentBuffs.Count == 0)
            {
                stringBuilder.AppendLine(Spellwright.GetTranslation("PlayerStatus", "NoPermamentBuffs").Value);
            }
            else
            {
                stringBuilder.AppendLine(Spellwright.GetTranslation("PlayerStatus", "HavePermamentBuffs").Value);
                foreach (int buffId in buffPlayer.PermamentBuffs)
                {
                    var buffName = Lang.GetBuffName(buffId);
                    var buffDescription = Lang.GetBuffDescription(buffId);
                    var line = $"{buffName} - {buffDescription}";
                    stringBuilder.AppendLine(line);
                }
            }

            return stringBuilder.ToString();
        }
    }
}
