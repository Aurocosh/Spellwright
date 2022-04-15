using Spellwright.Extensions;
using Spellwright.Network.Base;
using Terraria;

namespace Spellwright.Network.RoutedHandlers.Buffs
{
    internal class OtherPlayerClearBuffsHandler : RoutedPacketHandler<int[]>
    {
        protected override void HandleData(int[] data, byte fromWho, bool fromServer)
        {
            Player player = Main.LocalPlayer;
            player.ClearBuffs(data);
        }
    }
}
