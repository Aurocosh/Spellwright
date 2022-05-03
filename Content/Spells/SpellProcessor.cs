using Microsoft.Xna.Framework;
using Spellwright.Common.Players;
using Spellwright.Content.Items;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Modifiers;
using Spellwright.Core.Spells;
using Spellwright.ExecutablePackets.Broadcast.DustSpawners;
using Spellwright.Extensions;
using Spellwright.Network;
using System.Text;
using Terraria;
using Terraria.Audio;
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
            if (!spellPlayer.KnownSpells.Contains(spell.Type))
                return SpellCastResult.IncantationInvalid;

            var unlockResult = CheckUnlock(spellStructure, spell, spellPlayer);
            if (unlockResult != SpellCastResult.Success)
                return unlockResult;

            if (!CheckLevels(spell.SpellLevel, spellPlayer.PlayerLevel, spellStructure.SpellModifiers))
                return SpellCastResult.LevelTooLow;

            if (!spell.IsModifiersApplicable(spellStructure.SpellModifiers))
                return SpellCastResult.ModifiersInvalid;

            Player player = Main.LocalPlayer;
            if (!spell.ProcessExtraData(player, spellStructure, out object extraData))
                return SpellCastResult.ArgumentInvalid;

            var costModifier = spell.GetCostModifier(spellStructure.SpellModifiers);
            var spellData = new SpellData(spellStructure.SpellModifiers, spellStructure.Argument, costModifier, extraData);

            int playerLevel = spellPlayer.PlayerLevel;
            if (!spell.ConsumeReagentsCast(player, playerLevel, spellData))
                return SpellCastResult.NotEnoughReagents;

            if (spell.UseType == SpellType.Invocation)
            {
                spell.Cast(player, playerLevel, spellData);
                spell.PlayCastSound(player.Center);
                spell.DoCastEffect(player, playerLevel);
            }
            else if (spell.UseType == SpellType.Spell)
            {
                var itemType = ModContent.ItemType<SpellResonator>();

                var item = player.inventory[player.selectedItem];
                if (item.type != itemType)
                {
                    item = player.InventoryFindItem(itemType, InventoryArea.Hotbar);
                    if (item == null)
                        return SpellCastResult.NoTomeToBind;
                }

                var bookItem = item.ModItem as SpellResonator;
                bookItem.SpellUsesLeft = spell.GetGuaranteedUses(spellPlayer.PlayerLevel);
                bookItem.CurrentSpell = spell;
                bookItem.SpellData = spellData;
                bookItem.UpdateName();
                spell.PlayCastSound(player.Center);
                spell.DoCastEffect(player, playerLevel);
            }
            else
            {
                spellPlayer.CurrentCantrip = spell;
                spellPlayer.CantripData = spellData;
                spell.PlayCastSound(player.Center);
                spell.DoCastEffect(player, playerLevel);
            }
            return SpellCastResult.Success;
        }

        private static SpellCastResult CheckUnlock(SpellStructure spellStructure, ModSpell spell, SpellwrightPlayer spellPlayer)
        {
            if (spellStructure.HasModifier(SpellModifier.Unlock) && spellPlayer.UnlockedSpells.Contains(spell.Type))
                return SpellCastResult.AlreadyUnlocked;
            if (spell.UnlockCost == null)
                return SpellCastResult.Success;
            if (spell.SpellLevel > spellPlayer.PlayerLevel)
                return SpellCastResult.LevelTooLow;

            if (spellStructure.HasModifier(SpellModifier.Unlock))
            {
                Player player = Main.LocalPlayer;
                if (spell.UnlockCost.Consume(player, spellPlayer.PlayerLevel, SpellData.EmptyData))
                {
                    spellPlayer.UnlockedSpells.Add(spell.Type);
                    SoundEngine.PlaySound(SoundID.Item4, player.Center);

                    var spawner = new LevelUpDustSpawner(player, new int[] { spell.SpellLevel });
                    spawner.Execute();
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        ModNetHandler.levelUpDustHandler.Send(spawner);
                    return SpellCastResult.SpellUnlocked;
                }
                else
                {
                    var message = Spellwright.GetTranslation("CastResult", "NotEnoughReagents");
                    string costDescription = spell.UnlockCost.GetDescription(player, spellPlayer.PlayerLevel, SpellData.EmptyData);
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
            if (playerLevel <= 3 && spellModifiers.HasFlag(SpellModifier.Eternal))
                return false;
            return true;
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
            var spellParts = new StringBuilder();

            foreach (var word in words)
            {
                var modifier = SpellModifiersProcessor.GetModifier(word);

                if (modifier != null)
                    spellModifiers = spellModifiers.Add(modifier.Value);
                else
                    spellParts.AppendDelimited(" ", word);
            }

            var spellName = spellParts.ToString();
            return new SpellStructure(spellModifiers, spellName, spellArgument);
        }
    }
}
