using Spellwright.Spells.Base;
using Terraria.ID;

namespace Spellwright.Spells.WarpSpells
{
    internal class ConjureWaterSpell : LiquidSpawnSpell
    {
        public ConjureWaterSpell(string name, string incantation) : base(name, incantation, SpellType.Spell)
        {
            stability = 1f;
            liquidType = LiquidID.Water;
            useTimeMultiplier = 7f;
        }
    }
}