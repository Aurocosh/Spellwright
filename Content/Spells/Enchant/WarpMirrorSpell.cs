using Spellwright.Content.Items;
using Spellwright.Content.Spells.Base;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.Enchant
{
    internal class WarpMirrorSpell : BindMirrorSpell
    {
        public override void SetStaticDefaults()
        {
            UseType = SpellType.Invocation;
            itemType = ModContent.ItemType<WarpedMagicMirror>();
        }
    }
}