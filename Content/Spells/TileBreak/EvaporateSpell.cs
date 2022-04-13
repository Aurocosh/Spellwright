using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Types;
using Terraria;
using Terraria.ID;

namespace Spellwright.Content.Spells.TileBreak
{
    internal class EvaporateSpell : LiquidDestroySpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 0;
            UseType = SpellType.Invocation;
            liquidType = LiquidID.Water;
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            return base.Cast(player, playerLevel, spellData);
        }
    }
}