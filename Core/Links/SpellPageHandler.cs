﻿using Microsoft.Xna.Framework;
using Spellwright.Common.Players;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Description;
using Spellwright.Core.Spells;
using Spellwright.UI.Components.TextBox.Text;
using System.Text;
using Terraria;
using Terraria.Localization;

namespace Spellwright.Core.Links
{
    internal class SpellPageHandler : PageHandler
    {
        public SpellPageHandler()
        {
        }

        public override string ProcessLink(ref LinkData linkData, Player player)
        {
            var spellName = linkData.GetParameter("name");
            var spell = SpellLibrary.GetSpellByName(spellName);
            if (spell == null)
            {
                Spellwright.Instance.Logger.Error($"Spell name is invalid: {spellName}");
                return null;
            }

            var spellPlayer = player.GetModPlayer<SpellwrightPlayer>();

            bool isFavorite = spellPlayer.FavoriteSpells.Contains(spell.Type);
            bool hasFavToggle = linkData.HasParameter("toggleFavorite");
            if (hasFavToggle)
            {
                if (isFavorite)
                    spellPlayer.FavoriteSpells.Remove(spell.Type);
                else
                    spellPlayer.FavoriteSpells.Add(spell.Type);
                isFavorite = !isFavorite;

                linkData.RemoveParameter("toggleFavorite");
            }

            string name = spell.DisplayName.GetTranslation(Language.ActiveCulture);
            string formattedName = new FormattedText(name, Color.DarkGoldenrod).ToString();

            var builder = new StringBuilder();
            builder.AppendLine(formattedName);

            if (!spellPlayer.IsSpellUnlocked(spell))
            {
                var lockedMessage = GetFormText("Locked").WithColor(Color.IndianRed).ToString();
                builder.AppendLine(lockedMessage);
            }

            string favText = isFavorite ? "Favorite" : "NotFavorite";
            Color color = isFavorite ? Color.PaleVioletRed : Color.DarkGray;
            string favLine = new FormattedText(favText, color).WithLink("Spell").WithParam("name", spell.Name).WithParam("toggleFavorite").ToString();
            builder.AppendLine(favLine);

            var descriptionValues = spell.GetDescriptionValues(player, spellPlayer.PlayerLevel, SpellData.EmptyData, true);
            string description = spell.Description.GetTranslation(Language.ActiveCulture);
            descriptionValues.Add(new SpellParameter("Description", description));

            foreach (var value in descriptionValues)
            {
                var parameterName = Spellwright.GetTranslation("DescriptionParts", value.Name).Value + ":";
                var parameterValue = value.Value;
                parameterName = new FormattedText(parameterName, Color.Gray).ToString();

                if (value.Name == "SpellType")
                {
                    parameterValue = new FormattedText(parameterValue).WithLink("SpellType").WithParam("type", spell.UseType).ToString();
                }

                var descriptionPart = $"{parameterName} {parameterValue}";
                builder.AppendLine(descriptionPart);
            }

            return builder.ToString();
        }
    }
}