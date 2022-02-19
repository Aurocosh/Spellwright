using Spellwright.Content.Buffs.Spells;
using Spellwright.Items.Reagents;
using Spellwright.Spells.Base;
using Spellwright.Spells.SpellExtraData;
using Spellwright.Util;
using Terraria;
using Terraria.ModLoader;
using static Spellwright.Content.Buffs.Spells.ReactiveArmorBuff;

namespace Spellwright.Spells.BuffSpells
{
    internal class ReactiveArmorSpell : BuffSpell
    {
        protected override void DoExtraActions(Player player, int playerLevel)
        {
            base.DoExtraActions(player, playerLevel);
            var reactiveArmorPlayer = player.GetModPlayer<ReactiveArmorPlayer>();
            reactiveArmorPlayer.BonusDefense = 0;
            reactiveArmorPlayer.MaxBonusDefense = 4 + 2 * playerLevel;
        }

        public ReactiveArmorSpell(string name, string incantation) : base(name, incantation)
        {
            int buff = ModContent.BuffType<ReactiveArmorBuff>();
            AddEffect(buff, (playerLevel) => UtilTime.MinutesToTicks(10 + 3 * playerLevel));

            reagentType = ModContent.ItemType<RareSpellReagent>();
            reagentUseCost = 1;
            SetExtraReagentCost(SpellModifier.IsAoe, 8);
        }
    }
}
