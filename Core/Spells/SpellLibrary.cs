using Spellwright.Content.Spells.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spellwright.Core.Spells
{
    internal class SpellLibrary
    {
        private static readonly List<ModSpell> registeredSpells = new();
        private static readonly Dictionary<int, ModSpell> spellIdMap = new();
        private static readonly Dictionary<string, ModSpell> spellNameMap = new();
        private static readonly Dictionary<string, ModSpell> spellIncantationMap = new();

        public static void RegisterSpell(ModSpell modSpell)
        {
            registeredSpells.Add(modSpell);
        }

        public static void Refresh()
        {
            spellIdMap.Clear();
            spellNameMap.Clear();
            spellIncantationMap.Clear();
            foreach (ModSpell modSpell in registeredSpells)
            {
                spellIdMap.Add(modSpell.Type, modSpell);
                spellNameMap.Add(modSpell.Name, modSpell);
                var localIncantation = Spellwright.GetTranslation("Spells", modSpell.Name, "Incantation").Value;
                if (!localIncantation.StartsWith("Mods.Spellwright"))
                    SetSpellIncantation(localIncantation, modSpell);
                var defaultIncantation = GetDefaultIncantation(modSpell);
                if (defaultIncantation.ToLower() != localIncantation.ToLower())
                    SetSpellIncantation(defaultIncantation, modSpell);
            }
        }

        public static void Unload()
        {
            spellIdMap.Clear();
            spellNameMap.Clear();
            registeredSpells.Clear();
            spellIncantationMap.Clear();
        }

        public static IReadOnlyList<ModSpell> GetRegisteredSpells()
        {
            return registeredSpells;
        }

        public static ModSpell GetSpellById(int id)
        {
            if (spellIdMap.TryGetValue(id, out var modSpell))
                return modSpell;
            return null;
        }
        public static ModSpell GetSpellByName(string name)
        {
            if (spellNameMap.TryGetValue(name, out var modSpell))
                return modSpell;
            return null;
        }
        public static ModSpell GetSpellByIncantation(string incantation)
        {
            if (incantation == null)
                return null;
            if (!spellIncantationMap.TryGetValue(incantation.ToLower(), out ModSpell spell))
                return null;
            return spell;
        }

        private static void SetSpellIncantation(string newIncantation, ModSpell modSpell)
        {
            if (spellIncantationMap.TryGetValue(newIncantation, out var existingSpell))
                if (existingSpell != modSpell)
                    throw new Exception($"Spell incantation conflict. Two spells have identical incantation: {newIncantation}");
            spellIncantationMap.Add(newIncantation.ToLower(), modSpell);
        }

        private static string GetDefaultIncantation(ModSpell modSpell)
        {
            var name = modSpell.Name;
            if (name.EndsWith("Spell"))
                name = name.Substring(0, name.LastIndexOf("Spell"));

            var builder = new StringBuilder();
            foreach (char c in name)
            {
                if (char.IsUpper(c) && builder.Length > 0)
                    builder.Append(' ');
                builder.Append(c);
            }
            return builder.ToString();
        }
    }
}
