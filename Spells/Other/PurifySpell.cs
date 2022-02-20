using Spellwright.Extensions;
using Spellwright.Spells.Base;
using Spellwright.Spells.SpellExtraData;
using System.Collections.Generic;
using Terraria;

namespace Spellwright.Spells.WarpSpells
{
    internal class PurifySpell : Spell
    {
        public PurifySpell(string name, string incantation) : base(name, incantation, SpellType.Invocation)
        {

        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            var debuffIds = new List<int>();
            for (int i = 0; i < Player.MaxBuffs; i++)
            {
                int buffTime = player.buffTime[i];
                int buffType = player.buffType[i];
                bool isDebuff = Main.debuff[buffType];
                if (buffTime > 0 && isDebuff)
                    debuffIds.Add(buffType);
            }

            if (debuffIds.Count > 0)
                player.ClearBuffs(debuffIds);

            return true;
        }
    }
}