using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.SpellCosts;
using Spellwright.Content.Spells.Explosive;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Core.Spells
{
    internal static class SpellUnlockCosts
    {
        private static readonly Dictionary<int, SpellCost> spellUnlockCosts = new();

        public static void Initialize()
        {
            spellUnlockCosts.Clear();
            RegisterCost<DragonSpitSpell>(new SingleItemSpellCost(ItemID.Bomb, 20));
        }

        public static void Unload() => spellUnlockCosts.Clear();

        private static void RegisterCost<T>(SpellCost spellCost) where T : ModSpell
        {
            var modSpell = ModContent.GetInstance<T>() as T;
            spellUnlockCosts.Add(modSpell.Type, spellCost);
        }

        public static SpellCost GetUnlockCost(int spellTypeId)
        {
            if (spellUnlockCosts.TryGetValue(spellTypeId, out SpellCost spellCost))
                return spellCost;
            return null;
        }
    }
}
