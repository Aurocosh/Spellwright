using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Extensions;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.Other
{
    internal class PurifySpell : ModSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 7;
            UseType = SpellType.Invocation;

            UnlockCost = new SingleItemSpellCost(ItemID.PurificationPowder, 30);
            CastCost = new ReagentSpellCost(ModContent.ItemType<CommonSpellReagent>(), 1);
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