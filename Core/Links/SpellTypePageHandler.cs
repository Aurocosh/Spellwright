using Microsoft.Xna.Framework;
using Spellwright.Core.Links.Base;
using Spellwright.UI.Components.TextBox.Text;
using System.Text;
using Terraria;

namespace Spellwright.Core.Links
{
    internal class SpellTypePageHandler : PageHandler
    {
        internal enum SpellTypesCategories
        {
            Invocation = 0,
            Cantrip = 1,
            Echo = 2,
            All = 3
        }

        public override string ProcessLink(ref LinkData linkData, Player player)
        {
            var category = linkData.GetParameter("type", SpellTypesCategories.All);

            var builder = new StringBuilder();

            string title = GetFormText(category == SpellTypesCategories.All ? "SpellTypes" : category.ToString()).WithColor(Color.Purple).ToString();
            builder.AppendLine(title);
            builder.AppendLine();

            string shortSummary = GetFormText("ShortSummary").WithColor(Color.DarkGray).ToString();
            builder.AppendLine(shortSummary);

            if (category == SpellTypesCategories.All || category == SpellTypesCategories.Invocation)
            {
                builder.AppendLine(GetDescription(SpellTypesCategories.Invocation, "Summary"));
            }
            if (category == SpellTypesCategories.All || category == SpellTypesCategories.Cantrip)
            {
                builder.AppendLine(GetDescription(SpellTypesCategories.Cantrip, "Summary"));
            }
            if (category == SpellTypesCategories.All || category == SpellTypesCategories.Echo)
            {
                builder.AppendLine(GetDescription(SpellTypesCategories.Echo, "Summary"));
            }
            builder.AppendLine();

            string description = GetFormText("Description").WithColor(Color.DarkGray).ToString();
            builder.AppendLine(description);

            if (category == SpellTypesCategories.All || category == SpellTypesCategories.Invocation)
            {
                builder.AppendLine(GetDescription(SpellTypesCategories.Invocation, "Text"));
                builder.AppendLine();
            }
            if (category == SpellTypesCategories.All || category == SpellTypesCategories.Cantrip)
            {
                builder.AppendLine(GetDescription(SpellTypesCategories.Cantrip, "Text"));
                builder.AppendLine();
            }
            if (category == SpellTypesCategories.All || category == SpellTypesCategories.Echo)
            {
                builder.AppendLine(GetDescription(SpellTypesCategories.Echo, "Text"));
                builder.AppendLine();
            }

            return builder.ToString();
        }

        private string GetDescription(SpellTypesCategories category, string subtype)
        {
            var type = category.ToString();
            var name = GetFormText(type).WithColor(Color.MediumPurple);
            var text = GetTranslation(subtype + type);
            return $"{name} - {text}";
        }

    }
}
