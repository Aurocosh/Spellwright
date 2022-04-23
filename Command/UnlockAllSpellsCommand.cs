using Spellwright.Common.Players;
using Spellwright.Core.Spells;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace Spellwright.Command
{
    internal class UnlockAllSpellsCommand : ModCommand
    {
        public override CommandType Type => CommandType.Chat;

        public override string Command => "unlockAllSpells";

        public override string Usage
        {
            get { return "/unlockAllSpells"; }
        }

        public override string Description
        {
            get { return "Unlocks all spells"; }
        }

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            var spellIds = SpellLibrary.GetRegisteredSpells().Select(x => x.Type);

            var spellwrightPlayer = caller.Player.GetModPlayer<SpellwrightPlayer>();
            spellwrightPlayer.KnownSpells.UnionWith(spellIds);
            spellwrightPlayer.UnlockedSpells.UnionWith(spellIds);

            Main.NewText("Unlocked all spells");
        }
    }
}
