using System.Collections.Generic;
using System.Linq;

namespace Spellwright.Spells.SpellExtraData
{
    internal class SpellData
    {
        private readonly HashSet<SpellModifier> spellModifiers;
        public string Argument { get; }
        public object ExtraData { get; }

        public T GetExtraData<T>() where T : class => ExtraData as T;

        public bool HasModifier(SpellModifier spellModifier) => spellModifiers.Contains(spellModifier);
        public IReadOnlyList<SpellModifier> GetModifiers() => spellModifiers.ToList();

        public SpellData(IEnumerable<SpellModifier> spellModifiers, string argument, object extraSpellData)
        {
            this.spellModifiers = new HashSet<SpellModifier>(spellModifiers);
            Argument = argument;
            ExtraData = extraSpellData;
        }
    }
}
