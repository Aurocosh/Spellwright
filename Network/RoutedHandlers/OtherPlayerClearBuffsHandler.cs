using Spellwright.Network.Base;
using Terraria;

namespace Spellwright.Network.RoutedHandlers
{
    internal class OtherPlayerClearBuffsHandler : RoutedPacketHandler<int[]>
    {
        protected override void HandleData(int[] data, byte fromWho, bool fromServer)
        {
            Player player = Main.LocalPlayer;
            foreach (var buffId in data)
                player.ClearBuff(buffId);
        }
    }
}
