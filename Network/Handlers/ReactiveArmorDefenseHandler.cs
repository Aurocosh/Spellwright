using Terraria;
using static Spellwright.Content.Buffs.Spells.ReactiveArmorBuff;

namespace Spellwright.Network
{
    internal class ReactiveArmorDefenseHandler : IntSyncPacketHandler
    {
        protected override void HandleData(int data, byte fromWho, bool fromServer)
        {
            var modPlayer = Main.player[fromWho].GetModPlayer<ReactiveArmorPlayer>();
            modPlayer.BonusDefense = data;
        }
    }
}
