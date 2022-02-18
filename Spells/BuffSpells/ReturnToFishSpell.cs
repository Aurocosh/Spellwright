using Spellwright.Items.Reagents;
using Spellwright.Spells.Base;
using Spellwright.Spells.SpellExtraData;
using Spellwright.Util;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Spells.BuffSpells
{
    internal class ReturnToFishSpell : BuffSpell
    {
        public override int SpellLevel => 3;

        public override bool ConsumeReagents(Player player, int playerLevel, SpellData spellData)
        {
            if (!spellData.HasModifier(SpellModifier.IsAoe))
                return true;
            return base.ConsumeReagents(player, playerLevel, spellData);
        }
        public ReturnToFishSpell(string name, string incantation) : base(name, incantation)
        {
            static int durationGetter(int playerLevel) => UtilTime.MinutesToTicks(10 + 2 * playerLevel);

            AddEffect(BuffID.Gills, durationGetter);
            AddEffect(BuffID.Flipper, durationGetter);

            reagentType = ModContent.ItemType<CommonSpellReagent>();
            SetExtraReagentCost(SpellModifier.IsAoe, 1);
        }
    }
}
