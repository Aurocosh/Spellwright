using System.Collections.Generic;
using Terraria;

namespace Spellwright.Content.Spells.Base.Reagents
{
    internal class LevelMappedSpellCost : SpellCost
    {
        private readonly string invalidCostMessage;
        private readonly Dictionary<int, SpellCost> spellCostMap;

        public LevelMappedSpellCost()
        {
            invalidCostMessage = Spellwright.GetTranslation("Messages", "CostIsInvalid");
            spellCostMap = new();
        }
        public LevelMappedSpellCost(string invalidMessage)
        {
            invalidCostMessage = invalidMessage;
            spellCostMap = new();
        }

        public void SetSpellCost(int level, SpellCost spellCost)
        {
            spellCostMap[level] = spellCost;
        }

        public override bool Consume(Player player, int playerLevel, float costModifier, SpellData spellData)
        {
            if (!spellCostMap.TryGetValue(playerLevel, out var spellCost))
            {
                LastError = invalidCostMessage;
                return false;
            }
            if (!spellCost.Consume(player, playerLevel, costModifier, spellData))
            {
                LastError = spellCost.LastError;
                return false;
            }
            return true;
        }
    }
}
