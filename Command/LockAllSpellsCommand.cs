using Spellwright.Common.Players;
using Spellwright.Core.Spells;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace Spellwright.Command
{
    internal class LockAllSpellsCommand : ModCommand
    {
        public override CommandType Type => CommandType.Chat;

        public override string Command => "lockAllSpells";

        public override string Usage
        {
            get { return "/lockAllSpells"; }
        }

        public override string Description
        {
            get { return "Locks all spells"; }
        }

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            var spellIds = SpellLibrary.GetRegisteredSpells().Select(x => x.Type);

            var spellwrightPlayer = caller.Player.GetModPlayer<SpellwrightPlayer>();
            spellwrightPlayer.UnlockedSpells.ExceptWith(spellIds);

            Main.NewText("Locks all spells");
        }
    }
}
