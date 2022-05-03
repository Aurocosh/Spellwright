using System.Text;

namespace Spellwright.Extensions
{
    internal static class StringBuilderExtensions
    {
        public static void AppendDelimited(this StringBuilder stringBuilder, string delimiter, string value)
        {
            if (stringBuilder.Length > 0)
                stringBuilder.Append(delimiter);
            stringBuilder.Append(value);
        }
    }
}
