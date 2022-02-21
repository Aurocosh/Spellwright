using Terraria.UI;

namespace Spellwright.UI.States
{
    internal class SpellInputUiState : UIState
    {
        public SpellInput spellInput;

        public override void OnInitialize()
        {
            spellInput = new SpellInput();
            Append(spellInput);
        }
    }
}
