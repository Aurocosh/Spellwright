using Spellwright.Common.Players;
using Spellwright.Content.Items;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Modifiers;
using Spellwright.Core.Spells;
using Spellwright.Extensions;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells
{
    internal static class SpellProcessor
    {
        public static SpellCastResult ProcessCast(string incantationText)
        {
            SpellStructure spellStructure = ProcessIncantation(incantationText);
            if (spellStructure == null)
                return SpellCastResult.IncantationInvalid;
            if (spellStructure.SpellName.Length == 0)
                return SpellCastResult.IncantationInvalid;

            ModSpell spell = SpellLibrary.GetSpellByIncantation(spellStructure.SpellName);
            if (spell == null)
                return SpellCastResult.IncantationInvalid;

            SpellwrightPlayer spellwrightPlayer = SpellwrightPlayer.Instance;
            if (spell.SpellLevel > spellwrightPlayer.PlayerLevel)
                return SpellCastResult.LevelTooLow;
            if (!CheckMofifierLevels(spellwrightPlayer.PlayerLevel, spellStructure.SpellModifiers))
                return SpellCastResult.LevelTooLow;

            bool isModifiersApplicable = spell.IsModifiersApplicable(spellStructure.SpellModifiers);
            if (!isModifiersApplicable)
                return SpellCastResult.ModifiersInvalid;
            if (CountMultModifiers(spellStructure.SpellModifiers) > 1)
                return SpellCastResult.ModifiersInvalid;

            bool isSpellDataValid = spell.ProcessExtraData(spellStructure, out object extraData);
            if (!isSpellDataValid)
                return SpellCastResult.ArgumentInvalid;

            var costModifier = spell.GetCostModifier(spellStructure.SpellModifiers);
            var spellData = new SpellData(spellStructure.SpellModifiers, spellStructure.Argument, costModifier, extraData);
            if (spell.UseType == SpellType.Invocation)
            {
                Player player = Main.LocalPlayer;
                int playerLevel = spellwrightPlayer.PlayerLevel;
                if (!spell.ConsumeReagents(player, playerLevel, spellData))
                    return SpellCastResult.IncantationInvalid;
                spell.Cast(player, playerLevel, spellData);
            }
            else if (spell.UseType == SpellType.Spell)
            {
                var itemType = ModContent.ItemType<SpellweaverTome>();

                Player player = Main.LocalPlayer;
                var item = player.inventory[player.selectedItem];
                if (item.type != itemType)
                {
                    item = player.InventoryFindItem(itemType, InventoryArea.Hotbar);
                    if (item == null)
                        return SpellCastResult.NoTomeToBind;
                }

                int spellUses = spell.GetGuaranteedUses(spellwrightPlayer.PlayerLevel);
                if (spellData.HasModifier(SpellModifier.IsTwofold))
                    spellUses *= 2;
                if (spellData.HasModifier(SpellModifier.IsFivefold))
                    spellUses *= 5;
                if (spellData.HasModifier(SpellModifier.IsTenfold))
                    spellUses *= 10;
                if (spellData.HasModifier(SpellModifier.IsFiftyfold))
                    spellUses *= 50;

                var bookItem = item.ModItem as SpellweaverTome;
                bookItem.GuaranteedUsesLeft = spellUses;
                bookItem.CurrentSpell = spell;
                bookItem.SpellData = spellData;
            }
            else
            {
                spellwrightPlayer.CurrentCantrip = spell;
                spellwrightPlayer.CantripData = spellData;
            }
            return SpellCastResult.Success;
        }

        private static bool CheckMofifierLevels(int playerLevel, SpellModifier spellModifiers)
        {
            if (playerLevel < 5 && spellModifiers.HasFlag(SpellModifier.IsEternal))
                return false;

            if (playerLevel < 2 && spellModifiers.HasFlag(SpellModifier.IsTwofold))
                return false;
            if (playerLevel < 4 && spellModifiers.HasFlag(SpellModifier.IsFivefold))
                return false;
            if (playerLevel < 6 && spellModifiers.HasFlag(SpellModifier.IsTenfold))
                return false;
            if (playerLevel < 8 && spellModifiers.HasFlag(SpellModifier.IsFiftyfold))
                return false;

            return true;
        }

        private static int CountMultModifiers(SpellModifier spellModifier)
        {
            int count = 0;
            if (spellModifier.HasFlag(SpellModifier.IsTwofold))
                count += 1;
            if (spellModifier.HasFlag(SpellModifier.IsFivefold))
                count += 1;
            if (spellModifier.HasFlag(SpellModifier.IsTenfold))
                count += 1;
            if (spellModifier.HasFlag(SpellModifier.IsFiftyfold))
                count += 1;
            return count;
        }

        public static SpellStructure ProcessIncantation(string incantationText)
        {
            incantationText = incantationText.Trim().ToLower();

            var incantationParts = incantationText.Split(new[] { ':' }, 2);
            if (incantationParts.Length == 0)
                return null;

            string spellFunctionalPart = incantationParts[0];
            string spellArgument = incantationParts.Length > 1 ? incantationParts[1].Trim() : "";

            var words = spellFunctionalPart.Split(new[] { ' ' });
            if (words.Length == 0)
                return null;

            SpellModifier spellModifiers = SpellModifier.None;
            var spellParts = new List<string>();

            foreach (var word in words)
            {
                var modifier = SpellModifiersProcessor.GetModifier(word);

                if (modifier != null)
                    spellModifiers = spellModifiers.Add(modifier.Value);
                else
                    spellParts.Add(word);
            }

            var spellName = string.Join(' ', spellParts);
            return new SpellStructure(spellModifiers, spellName, spellArgument);
        }
    }
}
