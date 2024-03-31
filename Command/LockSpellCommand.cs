using Spellwright.Common.Players;
using Spellwright.Core.Spells;
using Terraria;
using Terraria.ModLoader;

namespace Spellwright.Command
{
    internal class LockSpellCommand : ModCommand
    {
        public override CommandType Type => CommandType.Chat;

        public override string Command => "lockSpell";

        public override string Usage
        {
            get { return "/lockSpell"; }
        }

        public override string Description
        {
            get { return "locks spell"; }
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
                spellwrightPlayer.UnlockedSpells.Remove(spellId.Type);

                Main.NewText("Locked spell");
            }
            else
                Main.NewText("Spell not found");
        }
    }
}
