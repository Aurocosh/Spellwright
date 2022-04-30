﻿using Microsoft.Xna.Framework;
using Spellwright.Common.Players;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.SpellCosts.Reagent;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.Movement
{
    internal class FlashStepSpell : ModSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 2;
            UseType = SpellType.Cantrip;

            CastCost = new ReagentSpellCost(ModContent.ItemType<RareSpellReagent>(), 1);
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData, IEntitySource source, Vector2 position, Vector2 direction)
        {
            SpellwrightDashPlayer dashPlayer = player.GetModPlayer<SpellwrightDashPlayer>();
            if (!dashPlayer.CanUseDash())
                return false;

            var newVelocity = new Vector2(player.direction * 20, 1);
            dashPlayer.Dash(newVelocity, 60);

            return true;
        }
    }
}