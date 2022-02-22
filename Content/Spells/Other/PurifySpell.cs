using Spellwright.Content.Spells.Base;
using Spellwright.Extensions;
using System.Collections.Generic;
using Terraria;

namespace Spellwright.Content.Spells.Other
{
    internal class PurifySpell : ModSpell
    {
        public override void SetStaticDefaults()
        {
            UseType = SpellType.Invocation;
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