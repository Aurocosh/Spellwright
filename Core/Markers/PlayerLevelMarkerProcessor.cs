using Spellwright.Common.Players;
using Spellwright.UI.Components.TextBox.MarkerProcessors.Base;
using Terraria;

namespace Spellwright.Core.Markers
{
    internal class PlayerLevelMarkerProcessor : MarkerProcessor
    {
        public override string ProcessMarker(MarkerData markerData, Player player)
        {
            var spellPlayer = player.GetModPlayer<SpellwrightPlayer>();
            return spellPlayer.PlayerLevel.ToString();
        }
    }
}
