using Microsoft.Xna.Framework;
using Spellwright.Content.Spells.Base.Modifiers;
using Spellwright.Core.Links.Base;
using Spellwright.UI.Components.TextBox.Text;
using System.Text;
using Terraria;

namespace Spellwright.Core.Links
{
    internal class SpellModifierPageHandler : PageHandler
    {
        public override string ProcessLink(ref LinkData linkData, Player player)
        {
            var modifier = linkData.GetParameter("type", SpellModifier.None);

            var builder = new StringBuilder();

            string title = GetFormText(modifier == SpellModifier.None ? "SpellModifiers" : modifier.ToString()).WithColor(Color.Purple).ToString();
            builder.AppendLine(title);
            builder.AppendLine();

            string description = GetFormText("Description").WithColor(Color.DarkGray).ToString();
            builder.AppendLine(description);

            if (modifier == SpellModifier.None || modifier == SpellModifier.Unlock)
            {
                builder.AppendLine(GetDescription(SpellModifier.Unlock, "Text"));
                builder.AppendLine();
            }
            if (modifier == SpellModifier.None || modifier == SpellModifier.Area)
            {
                builder.AppendLine(GetDescription(SpellModifier.Area, "Text"));
                builder.AppendLine();
            }
            if (modifier == SpellModifier.None || modifier == SpellModifier.Eternal)
            {
                builder.AppendLine(GetDescription(SpellModifier.Eternal, "Text"));
                builder.AppendLine();
            }
            if (modifier == SpellModifier.None || modifier == SpellModifier.Dispel)
            {
                builder.AppendLine(GetDescription(SpellModifier.Dispel, "Text"));
                builder.AppendLine();
            }

            return builder.ToString();
        }

        private string GetDescription(SpellModifier modifier, string subtype)
        {
            var type = modifier.ToString();
            var name = GetFormText(type).WithColor(Color.MediumPurple);
            var text = GetTranslation(subtype + type);
            return $"{name} - {text}";
        }
    }
}
