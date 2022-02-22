using Spellwright.Content.Buffs.Spells;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using Terraria;
using Terraria.ModLoader;
using static Spellwright.Content.Buffs.Spells.ReactiveArmorBuff;

namespace Spellwright.Content.Spells.BuffSpells
{
    internal class ReactiveArmorSpell : BuffSpell
    {
        public override void SetStaticDefaults()
        {
            int buff = ModContent.BuffType<ReactiveArmorBuff>();
            AddEffect(buff, (playerLevel) => UtilTime.MinutesToTicks(10 + 3 * playerLevel));

            reagentType = ModContent.ItemType<RareSpellReagent>();
            reagentUseCost = 1;
            SetExtraReagentCost(SpellModifier.IsAoe, 8);
        }
        protected override void DoExtraActions(Player player, int playerLevel)
        {
            base.DoExtraActions(player, playerLevel);
            var reactiveArmorPlayer = player.GetModPlayer<ReactiveArmorPlayer>();
            reactiveArmorPlayer.BonusDefense = 0;
            reactiveArmorPlayer.MaxBonusDefense = 4 + 2 * playerLevel;
        }
    }
}
