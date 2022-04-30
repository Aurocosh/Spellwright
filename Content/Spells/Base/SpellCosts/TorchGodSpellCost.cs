using Terraria;

namespace Spellwright.Content.Spells.Base.SpellCosts
{
    internal class TorchGodSpellCost : SpellCost
    {
        public override bool Consume(Player player, int playerLevel, SpellData spellData)
        {
            if (player.unlockedBiomeTorches)
                return true;

            LastError = Spellwright.GetTranslation("SpellCost", "TorchGod").Value;
            return false;
        }

        public override string GetDescription(Player player, int playerLevel, SpellData spellData)
        {
            return Spellwright.GetTranslation("SpellCost", "TorchGod").Value;
        }
    }
}
