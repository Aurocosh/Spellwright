using Spellwright.Common.Players;
using Spellwright.Network.Base;
using Terraria;

namespace Spellwright.Network.Handlers
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
