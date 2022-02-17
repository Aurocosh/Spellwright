using Spellwright.Spells.Base;
using Terraria.ID;

namespace Spellwright.Spells.WarpSpells
{
    internal class ConjureLavaSpell : LiquidSpawnSpell
    {
        public override int GetGuaranteedUses(int playerLevel) => 20 + 2 * playerLevel;
        public ConjureLavaSpell(string name, string incantation) : base(name, incantation, SpellType.Spell)
        {
            liquidType = LiquidID.Lava;
            useTimeMultiplier = 7f;
        }
    }
}