using Microsoft.Xna.Framework;
using Spellwright.UI.Components.TextBox.MarkerProcessors.Base;
using Spellwright.UI.Components.TextBox.Text;
using Terraria;

namespace Spellwright.Core.Markers
{
    internal class ItemLinkMarkerProcessor : MarkerProcessor
    {
        public override string ProcessMarker(MarkerData markerData, Player player)
        {
            string itemName = markerData.GetParameter("name", markerData.Id);
            return new FormattedText(markerData.Text, Color.DarkSlateBlue).WithLink("ModItem").WithParam("name", itemName).ToString();
        }
    }
}
