using Spellwright.Network.Base;
using Terraria;
using static Spellwright.Content.Buffs.Spells.Defensive.ReactiveArmorBuff;

namespace Spellwright.Network.Sync
{
    internal class ReactiveArmorMaxDefenseHandler : SyncPacketHandler<int>
    {
        protected override void HandleData(SyncData<int> data, byte fromWho, bool fromServer)
        {
            var modPlayer = Main.player[data.PlayerId].GetModPlayer<ReactiveArmorPlayer>();
            modPlayer.MaxBonusDefense = data.Value;
        }
    }
}
