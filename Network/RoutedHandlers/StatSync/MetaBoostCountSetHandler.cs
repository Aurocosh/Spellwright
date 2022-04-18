using Spellwright.Common.Players;
using Spellwright.Network.Base;
using Terraria;

namespace Spellwright.Network.RoutedHandlers.StatSync
{
    internal class MetaBoostCountSetHandler : RoutedPacketHandler<int>
    {
        protected override void HandleData(int boostValue, byte fromWho, bool fromServer)
        {
            Player player = Main.LocalPlayer;
            var statPlayer = player.GetModPlayer<SpellwrightStatPlayer>();
            statPlayer.MetaBoostCount = boostValue;
        }
    }
}
