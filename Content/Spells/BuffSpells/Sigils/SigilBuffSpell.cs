using Spellwright.Content.Buffs.Spells.Sigils;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Modifiers;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Network.RoutedHandlers.PermanentEffects;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.BuffSpells.Sigils
{
    internal abstract class SigilBuffSpell : BuffSpell
    {
        protected SigilBuffSpell()
        {
            RemoveApplicableModifier(SpellModifier.Area);
        }

        protected override void ApplyEffect(IEnumerable<Player> affectedPlayers, int playerLevel, SpellData spellData)
        {
            var sigilIds = new int[] {
                ModContent.BuffType<SigilOfBerserkerBuff>(),
                ModContent.BuffType<SigilOfLegionBuff>(),
                ModContent.BuffType<SigilOfSageBuff>(),
                ModContent.BuffType<SigilOfSniperBuff>()
            };

            foreach (var affectedPlayer in affectedPlayers)
                new PlayerRemovePermanentEffectAction(affectedPlayer.whoAmI, sigilIds).Execute();

            base.ApplyEffect(affectedPlayers, playerLevel, spellData);
        }
    }
}
