using Spellwright.Network.Base;
using Terraria;
using static Spellwright.Content.Buffs.Spells.ReactiveArmorBuff;

namespace Spellwright.Network.Handlers
{
    internal class ReactiveArmorMaxDefenseHandler : IntSyncPacketHandler
    {
        protected override void HandleData(int data, byte fromWho, bool fromServer)
        {
            var modPlayer = Main.player[fromWho].GetModPlayer<ReactiveArmorPlayer>();
            modPlayer.MaxBonusDefense = data;
        }
    }
}
