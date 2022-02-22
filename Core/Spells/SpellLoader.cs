using Spellwright.Content.Spells.Base;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace Spellwright.Core.Spells
{
    internal static class SpellLoader
    {
        private static int nextSpellId = 0;
        private static readonly List<ModSpell> spells = new();

        public static int SpellCount => nextSpellId;
        internal static ModSpell GetSpell(int type) => type < SpellCount ? spells[type] : null;
        internal static IReadOnlyList<ModSpell> GetAllSpells() => spells;

        internal static int ReserveBuffID()
        {
            if (ModNet.AllowVanillaClients)
                throw new Exception("Adding spells breaks vanilla client compatibility");

            int reserveID = nextSpellId;
            nextSpellId++;
            return reserveID;
        }

        internal static int RegisterSpell(ModSpell modSpell)
        {
            int type = ReserveBuffID();
            spells.Add(modSpell);
            return type;
        }

        internal static void Unload()
        {
            spells.Clear();
            nextSpellId = 0;
        }
    }
}
