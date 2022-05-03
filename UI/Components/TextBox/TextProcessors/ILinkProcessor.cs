using Spellwright.UI.Components.TextBox.StateData;

namespace Spellwright.UI.Components.TextBox.TextProcessors
{
    internal interface ILinkProcessor
    {
        LinkTextResult Process(string linkText);
    }
}
