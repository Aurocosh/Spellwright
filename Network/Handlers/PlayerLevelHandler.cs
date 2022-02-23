using Spellwright.Common.Players;
using Terraria;

namespace Spellwright.Network
{
    internal class PlayerLevelHandler : IntSyncPacketHandler
    {
        protected override void HandleData(int data, byte fromWho, bool fromServer)
        {
            var modPlayer = Main.player[fromWho].GetModPlayer<SpellwrightPlayer>();
            modPlayer.PlayerLevel = data;
        }
    }
}
