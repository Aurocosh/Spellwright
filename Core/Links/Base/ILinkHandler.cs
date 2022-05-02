using Terraria;

namespace Spellwright.Core.Links
{
    internal interface ILinkHandler
    {
        string Type { get; }
        string ProcessLink(string link, Player player);
    }
}
