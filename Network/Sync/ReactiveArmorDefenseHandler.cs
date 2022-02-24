using Spellwright.Network.Base;
using Terraria;
using static Spellwright.Content.Buffs.Spells.ReactiveArmorBuff;

namespace Spellwright.Network.Sync
{
    internal class ReactiveArmorDefenseHandler : SyncPacketHandler<int>
    {
        protected override void HandleData(SyncData<int> data, byte fromWho, bool fromServer)
        {
            var modPlayer = Main.player[data.PlayerId].GetModPlayer<ReactiveArmorPlayer>();
            modPlayer.BonusDefense = data.Value;
        }
    }
}
