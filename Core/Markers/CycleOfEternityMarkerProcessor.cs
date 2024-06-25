using Spellwright.Common.Players;
using Spellwright.UI.Components.TextBox.MarkerProcessors.Base;
using Terraria;

namespace Spellwright.Core.Markers
{
    internal class CycleOfEternityMarkerProcessor : MarkerProcessor
    {
        public override string ProcessMarker(MarkerData markerData, Player player)
        {
            var buffPlayer = player.GetModPlayer<SpellwrightBuffPlayer>();
            if (buffPlayer.CycleOfEternity)
                return GetTranslation("ActiveMessage").Value + "\n";
            return "";
        }
    }
}
