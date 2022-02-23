using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.BuffSpells
{
    internal class ReturnToFishSpell : BuffSpell
    {
        public override int SpellLevel => 3;

        public override void SetStaticDefaults()
        {
            static int durationGetter(int playerLevel) => UtilTime.MinutesToTicks(10 + 2 * playerLevel);

            AddEffect(BuffID.Gills, durationGetter);
            AddEffect(BuffID.Flipper, durationGetter);

            reagentType = ModContent.ItemType<CommonSpellReagent>();
        }
        public override bool ConsumeReagents(Player player, int playerLevel, SpellData spellData)
        {
            if (!spellData.HasModifier(SpellModifier.IsAoe))
                return true;
            return base.ConsumeReagents(player, playerLevel, spellData);
        }
    }
}
