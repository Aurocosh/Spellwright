using Spellwright.Spells.Base;
using Spellwright.Spells.SpellExtraData;
using Terraria;

namespace Spellwright.Spells.BuffSpells
{
    internal class BloodyRageSpell : Spell
    {
        protected override int GetDamage(int playerLevel) => damage + 10 * playerLevel;

        public BloodyRageSpell(string name, string incantation) : base(name, incantation, SpellType.Invocation)
        {
            damage = 50;
            RemoveApplicableModifier(SpellModifier.IsAoe);
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            player.statLife = 20;

            //// Make dust 70 times for a cool effect. This dust is the dust at the destination.
            //for (int d = 0; d < 70; d++)
            //    Dust.NewDust(player.position, player.width, player.height, 15, 0f, 0f, 150, default, 1.5f);

            return true;
        }
    }
}