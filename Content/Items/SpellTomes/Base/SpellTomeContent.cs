using Spellwright.Content.Spells.Base;
using Spellwright.MyLibs.Randoms;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace Spellwright.Content.Items.SpellTomes.Base
{
    public class SpellTomeContent
    {
        public readonly List<ModSpell> Spells = new();
        public readonly DistributedRandom<int> SpellCounts = new();
        public readonly Dictionary<ModSpell, double> SpellDistributions = new();

        public void AddCount(int count, double distribution = 1)
        {
            SpellCounts.Add(count, distribution);
        }

        public void AddSpell<T>(double distribution = 1)
            where T : ModSpell
        {
            var modSpell = ModContent.GetInstance<T>();
            Spells.Add(modSpell);
            SpellDistributions.Add(modSpell, distribution);
        }
    }
}
