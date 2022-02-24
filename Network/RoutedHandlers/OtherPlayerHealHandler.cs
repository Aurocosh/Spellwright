using Spellwright.Network.Base;
using Terraria;

namespace Spellwright.Network.RoutedHandlers
{
    internal class OtherPlayerHealHandler : RoutedPacketHandler<int>
    {
        protected override void HandleData(int healValue, byte fromWho, bool fromServer)
        {
            Player player = Main.LocalPlayer;
            player.statLife += healValue;
            player.HealEffect(healValue);
        }
    }
}
