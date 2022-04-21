using Spellwright.Content.Buffs.Spells;
using Spellwright.Content.Spells.Base.Types;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.BuffSpells
{
    internal class StateLockSpell : BuffSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 8;

            int buff = ModContent.BuffType<StateLockBuff>();
            AddEffect(buff, (playerLevel) => 10000);
        }
    }
}
