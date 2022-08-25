using Microsoft.Xna.Framework;
using Spellwright.Core.Links.Base;
using Spellwright.UI.Components.TextBox.Text;
using System.Text;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Spellwright.Core.Links
{
    internal class ModItemPageHandler : PageHandler
    {
        public ModItemPageHandler()
        {
        }

        public override string ProcessLink(ref LinkData linkData, Player player)
        {
            var itemName = linkData.GetParameter("name");
            if (!ModContent.TryFind(Spellwright.Instance.Name, itemName, out ModItem modItem))
            {
                Spellwright.Instance.Logger.Error($"Item not found: {itemName}");
                return null;
            }

            var builder = new StringBuilder();
            string name = modItem.DisplayName.GetTranslation(Language.ActiveCulture);
            string formattedName = new FormattedText(name, Color.DarkGoldenrod).ToString();
            builder.AppendLine(formattedName);

            var typeWord = GetFormText("Type").WithColor(Color.Gray).ToString();
            var types = GetTranslation(itemName, "Type").Value;
            var typeString = $"{typeWord}: {types}";
            builder.AppendLine(typeString);

            //var tooltipWord = GetFormText("Tooltip").WithColor(Color.Gray).ToString();
            //string tooltip = modItem.Tooltip.GetTranslation(Language.ActiveCulture).Replace('\n', ' ').Trim();
            //var tooltipString = $"{tooltipWord}: {tooltip}";
            //builder.AppendLine(tooltipString);

            var descriptionWord = GetFormText("Description").WithColor(Color.Gray).ToString(); ;
            var description = GetTranslation(itemName, "Description").Value;
            var descriptionString = $"{descriptionWord}: {description}";
            builder.AppendLine(descriptionString);

            return builder.ToString();
        }
    }
}
