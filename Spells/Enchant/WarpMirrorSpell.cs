using Spellwright.Items;
using Terraria.ModLoader;

namespace Spellwright.Spells.WarpSpells
{
    internal class WarpMirrorSpell : BindMirrorSpell
    {
        public WarpMirrorSpell(string name, string incantation) : base(name, incantation)
        {
            itemType = ModContent.ItemType<WarpedMagicMirror>();
        }
    }
}