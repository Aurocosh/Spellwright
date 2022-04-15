using Spellwright.Content.Buffs.Spells;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.BuffSpells
{
    internal class SelfDefenseHexSpell : BuffSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 6;
            AddEffect(ModContent.BuffType<SelfDefenseHexBuff>(), (playerLevel) => UtilTime.MinutesToTicks(4 + playerLevel));
        }
    }
}
