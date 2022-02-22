using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Types;
using Terraria.ID;

namespace Spellwright.Content.Spells.LiquidSpawn
{
    internal class ConjureLavaSpell : LiquidSpawnSpell
    {
        public override int GetGuaranteedUses(int playerLevel) => 20 + 2 * playerLevel;

        public override void SetStaticDefaults()
        {
            UseType = SpellType.Spell;

            liquidType = LiquidID.Lava;
            useTimeMultiplier = 7f;
        }
    }
}