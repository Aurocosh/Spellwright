using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Types;
using Terraria.ID;

namespace Spellwright.Content.Spells.LiquidSpawn
{
    internal class ConjureWaterSpell : LiquidSpawnSpell
    {
        public override void SetStaticDefaults()
        {
            UseType = SpellType.Spell;
            stability = 1f;
            liquidType = LiquidID.Water;
            useTimeMultiplier = 7f;
        }
    }
}