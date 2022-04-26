using Spellwright.Common.Players;
using Spellwright.Core.Spells;
using Terraria;
using Terraria.ModLoader;

namespace Spellwright.Command
{
    internal class UnlockSpellCommand : ModCommand
    {
        public override CommandType Type => CommandType.Chat;

        public override string Command => "unlockSpell";

        public override string Usage
        {
            get { return "/unlockSpell"; }
        }

        public override string Description
        {
            get { return "unlocks spell"; }
        }

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (args.Length < 1)
                return;

            var incantation = string.Join(" ", args);
            var spellId = SpellLibrary.GetSpellByIncantation(incantation);
            if (spellId != null)
            {
                var spellwrightPlayer = caller.Player.GetModPlayer<SpellwrightPlayer>();
                spellwrightPlayer.KnownSpells.Add(spellId.Type);
                spellwrightPlayer.UnlockedSpells.Add(spellId.Type);

                Main.NewText("Unlocked spell");
            }
            else
                Main.NewText("Spell not found");
        }
    }
}
