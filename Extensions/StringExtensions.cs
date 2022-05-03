using Spellwright.UI.Components.TextBox.Text;

namespace Spellwright.Extensions
{
    internal static class StringExtensions
    {
        public static FormattedText AsFormText(this string value)
        {
            return new FormattedText(value);
        }
    }
}
