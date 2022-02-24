using Spellwright.Data;
using Spellwright.Network.Base;
using Terraria;

namespace Spellwright.Network.RoutedHandlers
{
    internal class OtherPlayerAddBuffsHandler : RoutedPacketHandler<BuffData[]>
    {
        protected override void HandleData(BuffData[] data, byte fromWho, bool fromServer)
        {
            Player player = Main.LocalPlayer;
            foreach (var buffData in data)
                player.AddBuff(buffData.Type, buffData.Duration);
        }
    }
}
