using Spellwright.Content.Buffs.Spells;
using Spellwright.Spells.Base;
using Spellwright.Spells.SpellExtraData;
using Spellwright.Util;
using Terraria.ModLoader;

namespace Spellwright.Spells.BuffSpells
{
    internal class WillOfTorchGodSpell : BuffSpell
    {
        public WillOfTorchGodSpell(string name, string incantation) : base(name, incantation)
        {
            int buffId = ModContent.BuffType<WillOfTorchGodBuff>();
            AddEffect(buffId, (playerLevel) => UtilTime.MinutesToTicks(10 + 2 * playerLevel));
            RemoveApplicableModifier(SpellModifier.IsAoe);
        }
    }
}
