using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using Terraria.ID;

namespace Spellwright.Content.Spells.BuffSpells
{
    internal class EyesOfProfitSpell : BuffSpell
    {
        public override void SetStaticDefaults()
        {
            reagentType = ItemID.GoldCoin;
            reagentUseCost = 2;

            AddEffect(BuffID.Spelunker, (playerLevel) => UtilTime.MinutesToTicks(10 + 2 * playerLevel));
        }
    }
}
