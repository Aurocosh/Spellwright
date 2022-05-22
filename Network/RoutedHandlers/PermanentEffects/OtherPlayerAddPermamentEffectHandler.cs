using Spellwright.Common.Players;
using Spellwright.Network.Base;
using Terraria;

namespace Spellwright.Network.RoutedHandlers.PermanentEffects
{
    internal class OtherPlayerAddPermanentEffectHandler : RoutedPacketHandler<int[]>
    {
        protected override void HandleData(int[] data, byte fromWho, bool fromServer)
        {
            Player player = Main.LocalPlayer;
            var modPlayer = player.GetModPlayer<SpellwrightBuffPlayer>();
            foreach (var effectId in data)
                modPlayer.PermanentBuffs.Add(effectId);
        }
    }
}
