using Microsoft.Xna.Framework;
using Spellwright.Common.Players;
using Spellwright.Content.Items;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Modifiers;
using Spellwright.Content.Spells.Base.SpellCosts;
using Spellwright.Core.Spells;
using Spellwright.ExecutablePackets.Broadcast.DustSpawners;
using Spellwright.Extensions;
using Spellwright.Network;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells
{
    internal static class SpellProcessor
    {
        public static SpellCastResult ProcessCast(string incantationText)
        {
            SpellStructure spellStructure = ProcessIncantation(incantationText);
            ModSpell spell = SpellLibrary.GetSpellByIncantation(spellStructure?.SpellName);
            if (spell == null)
                return SpellCastResult.IncantationInvalid;

            var spellPlayer = SpellwrightPlayer.Instance;
            var unlockResult = CheckUnlock(spellStructure, spell, spellPlayer);
            if (unlockResult != SpellCastResult.Success)
                return unlockResult;

            if (!CheckLevels(spell.SpellLevel, spellPlayer.PlayerLevel, spellStructure.SpellModifiers))
                return SpellCastResult.LevelTooLow;

            if (!spell.IsModifiersApplicable(spellStructure.SpellModifiers))
                return SpellCastResult.ModifiersInvalid;
            if (CountMultModifiers(spellStructure.SpellModifiers) > 1)
                return SpellCastResult.ModifiersInvalid;

            if (!spell.ProcessExtraData(spellStructure, out object extraData))
                return SpellCastResult.ArgumentInvalid;

            var costModifier = spell.GetCostModifier(spellStructure.SpellModifiers);
            var spellData = new SpellData(spellStructure.SpellModifiers, spellStructure.Argument, costModifier, extraData);
            if (spell.UseType == SpellType.Invocation)
            {
                Player player = Main.LocalPlayer;
                int playerLevel = spellPlayer.PlayerLevel;
                if (!spell.ConsumeReagents(player, playerLevel, spellData))
                    return SpellCastResult.NotEnoughReagents;
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

                int spellUses = spell.GetGuaranteedUses(spellPlayer.PlayerLevel);
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
                spellPlayer.CurrentCantrip = spell;
                spellPlayer.CantripData = spellData;
            }
            return SpellCastResult.Success;
        }

        private static SpellCastResult CheckUnlock(SpellStructure spellStructure, ModSpell spell, SpellwrightPlayer spellPlayer)
        {
            if (spellStructure.HasModifier(SpellModifier.IsUnlock) && spellPlayer.UnlockedSpells.Contains(spell.Type))
                return SpellCastResult.AlreadyUnlocked;
            SpellCost spellCost = SpellUnlockCosts.GetUnlockCost(spell.Type);
            if (spellCost == null)
                return SpellCastResult.Success;
            if (spell.SpellLevel > spellPlayer.PlayerLevel)
                return SpellCastResult.LevelTooLow;

            if (spellStructure.HasModifier(SpellModifier.IsUnlock))
            {
                Player player = Main.LocalPlayer;
                var spellData = new SpellData(SpellModifier.IsUnlock, "", 1, null);
                if (spellCost.Consume(player, spellPlayer.PlayerLevel, spellData))
                {
                    spellPlayer.UnlockedSpells.Add(spell.Type);

                    var spawner = new LevelUpDustSpawner(player, spell.SpellLevel);
                    spawner.Execute();
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        ModNetHandler.levelUpDustHandler.Send(spawner);
                    return SpellCastResult.SpellUnlocked;
                }
                else
                {
                    var message = Spellwright.GetTranslation("CastResult", "NotEnoughReagents");
                    string costDescription = spellCost.GetDescription(player, spellPlayer.PlayerLevel, spellData);
                    Main.NewText(message.Format(costDescription), Color.Orange);
                    return SpellCastResult.CustomError;
                }
            }
            else if (!spellPlayer.UnlockedSpells.Contains(spell.Type))
            {
                return SpellCastResult.NotUnlocked;
            }

            return SpellCastResult.Success;
        }

        private static bool CheckLevels(int spellLevel, int playerLevel, SpellModifier spellModifiers)
        {
            if (spellLevel > playerLevel)
                return false;

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
