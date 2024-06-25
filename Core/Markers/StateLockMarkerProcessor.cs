using Spellwright.Common.Players;
using Spellwright.UI.Components.TextBox.MarkerProcessors.Base;
using Terraria;

namespace Spellwright.Core.Markers
{
    internal class StateLockMarkerProcessor : MarkerProcessor
    {
        public override string ProcessMarker(MarkerData markerData, Player player)
        {
            var buffPlayer = player.GetModPlayer<SpellwrightBuffPlayer>();
            if (buffPlayer.StateLockCount > 0)
                return GetTranslation("ActiveMessage").Format(buffPlayer.StateLockCount) + "\n";
            return "";
        }
    }
}
