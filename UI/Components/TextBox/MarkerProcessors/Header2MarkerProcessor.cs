using Microsoft.Xna.Framework;
using Spellwright.UI.Components.TextBox.MarkerProcessors.Base;
using Spellwright.UI.Components.TextBox.Text;
using Terraria;

namespace Spellwright.UI.Components.TextBox.MarkerProcessors
{
    internal class Header2MarkerProcessor : MarkerProcessor
    {
        public override string ProcessMarker(MarkerData markerData, Player player)
        {
            return new FormattedText(markerData.Text).WithColor(Color.DarkGray).ToString();
        }
    }
}
