using Spellwright.Common.Players;
using Spellwright.UI.Components.TextBox.MarkerProcessors.Base;
using System.Text;
using Terraria;

namespace Spellwright.Core.Markers
{
    internal class PermamentBuffListMarkerProcessor : MarkerProcessor
    {
        public override string ProcessMarker(MarkerData markerData, Player player)
        {
            var stringBuilder = new StringBuilder();

            var buffPlayer = player.GetModPlayer<SpellwrightBuffPlayer>();
            if (buffPlayer.PermanentBuffs.Count == 0)
            {
                stringBuilder.AppendLine(GetTranslation("NoPermanentBuffs").Value);
            }
            else
            {
                stringBuilder.AppendLine(GetTranslation("HavePermanentBuffs").Value);
                foreach (int buffId in buffPlayer.PermanentBuffs)
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
