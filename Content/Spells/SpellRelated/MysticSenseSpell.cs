using Spellwright.Common.Players;
using Spellwright.Content.Spells.Base;
using Spellwright.UI.States;
using System.Collections.Generic;
using Terraria;

namespace Spellwright.Content.Spells.Special
{
    internal class MysticSenseSpell : ModSpell
    {
        public override void SetStaticDefaults()
        {
            UseType = SpellType.Invocation;
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            if (spellData == null)
                return false;

            var parts = new List<string>();

            var modPlayer = player.GetModPlayer<SpellwrightPlayer>();
            var message = Spellwright.GetTranslation("Spells", Name, "MyCurrentLevel").Format(modPlayer.PlayerLevel);
            parts.Add(message);

            var buffPlayer = player.GetModPlayer<SpellwrightBuffPlayer>();
            if (buffPlayer.PermamentBuffs.Count == 0)
            {
                parts.Add(Spellwright.GetTranslation("Spells", Name, "NoPermamentBuffs").Value);
            }
            else
            {
                var buffLines = new List<string>();
                buffLines.Add(Spellwright.GetTranslation("Spells", Name, "HavePermamentBuffs").Value);
                foreach (int buffId in buffPlayer.PermamentBuffs)
                {
                    var buffName = Lang.GetBuffName(buffId);
                    var buffDescription = Lang.GetBuffDescription(buffId);
                    var line = $"{buffName} - {buffDescription}";
                    buffLines.Add(line);
                }

                parts.Add(string.Join("\n", buffLines));
            }

            var result = string.Join("\n\n", parts);

            UIMessageState uiMessageState = Spellwright.Instance.uiMessageState;
            uiMessageState.SetMessage(result);
            Spellwright.Instance.userInterface.SetState(uiMessageState);

            return true;
        }
    }
}
