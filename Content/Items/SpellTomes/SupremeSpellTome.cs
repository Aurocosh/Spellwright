using Spellwright.Content.Items.SpellTomes.Base;
using Spellwright.Content.Spells.BuffSpells;
using Spellwright.Content.Spells.BuffSpells.Defensive;
using Spellwright.Content.Spells.BuffSpells.Sigils;
using Spellwright.Content.Spells.BuffSpells.Utility;
using Spellwright.Content.Spells.Enchant;
using Spellwright.Content.Spells.Herbs;
using Spellwright.Content.Spells.Items;
using Spellwright.Content.Spells.LiquidSpawn;
using Spellwright.Content.Spells.Minions;
using Spellwright.Content.Spells.Movement;
using Spellwright.Content.Spells.Other;
using Spellwright.Content.Spells.Warp;
using Terraria;
using Terraria.ID;

namespace Spellwright.Content.Items.SpellTomes
{
    public class SupremeSpellTome : SpellTome
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Supreme Spell Tome");
            Tooltip.SetDefault("Very valueable book that contains most rare and powerful spells. This tome contains spells ranging from level 7 to level 10.");

            var content = new SpellTomeContent();

            // Level 7
            content.AddSpell<ManaShieldSpell>();
            content.AddSpell<SoulNibblerSpell>();
            content.AddSpell<SigilOfLegionSpell>();
            content.AddSpell<SigilOfBerserkerSpell>();
            content.AddSpell<SigilOfSageSpell>();
            content.AddSpell<SigilOfSniperSpell>();
            content.AddSpell<PurifySpell>();

            // Level 8
            content.AddSpell<GravityDashSpell>();
            content.AddSpell<StateLockSpell>();
            content.AddSpell<VortexHandsSpell>();
            content.AddSpell<ReactiveArmorSpell>();
            content.AddSpell<ConjureLavaSpell>();

            // Level 9
            content.AddSpell<BindMirrorSpell>();
            content.AddSpell<AdventOfSummerSpell>();
            content.AddSpell<GreedyVortexSpell>();

            // Level 10
            content.AddSpell<HymnOfDiscordSpell>();
            content.AddSpell<MetabolicBoostSpell>();
            content.AddSpell<MagickaFairySpell>();

            Add(Type, content);
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.value = Item.buyPrice(0, 0, 0, 1);
            //Item.value = Item.buyPrice(1, 0, 0);
            Item.rare = ItemRarityID.Red;
        }
    }
}