using Spellwright.Players;
using Terraria;
using Terraria.ModLoader;

namespace Spellwright.Command
{
    internal class LevelEditCommand : ModCommand
    {
        public override CommandType Type => CommandType.Chat;

        public override string Command => "setMySpellLevel";

        public override string Usage
        {
            get { return "/setMySpellLevel"; }
        }

        public override string Description
        {
            get { return "Chenges spell level of current player"; }
        }

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (args.Length < 1)
                return;

            if (!int.TryParse(args[0], out int level))
                return;

            var spellwrightPlayer = caller.Player.GetModPlayer<SpellwrightPlayer>();
            spellwrightPlayer.PlayerLevel = level;

            Main.NewText("Changed level");
        }
    }
}
