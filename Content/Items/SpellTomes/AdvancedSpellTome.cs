using Spellwright.Content.Items.SpellTomes.Base;
using Spellwright.Content.Spells.BuffSpells;
using Spellwright.Content.Spells.BuffSpells.Defensive;
using Spellwright.Content.Spells.BuffSpells.Utility;
using Spellwright.Content.Spells.BuffSpells.Vanilla;
using Spellwright.Content.Spells.Explosive;
using Spellwright.Content.Spells.Herbs;
using Spellwright.Content.Spells.Projectiles;
using Spellwright.Content.Spells.Storage;
using Spellwright.Content.Spells.TileBreak;
using Spellwright.Content.Spells.TileSpawn;
using Spellwright.Content.Spells.Warp;
using Spellwright.Content.Spells.WorldEvents;
using Terraria;
using Terraria.ID;

namespace Spellwright.Content.Items.SpellTomes
{
    public class AdvancedSpellTome : SpellTome
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Advanced Spell Tome");
            Tooltip.SetDefault("A book that contains complex and potent mid level spells. This tome contains spells ranging from level 4 to level 6.");

            var content = new SpellTomeContent();

            // Level 4
            content.AddSpell<BaseJumpSpell>();
            content.AddSpell<BlockSpitterSpell>();
            content.AddSpell<BloodArrowSpell>();
            content.AddSpell<GossamerShoesSpell>();
            content.AddSpell<HellGateSpell>();
            content.AddSpell<MoneyTroughSpell>();
            content.AddSpell<RainCallSpell>();
            content.AddSpell<ReagentSubspaceSpell>();
            content.AddSpell<WallCrumblerSpell>();
            content.AddSpell<WallSpitterSpell>();

            // Level 5
            content.AddSpell<BurningSoulSpell>();
            content.AddSpell<CallOfTheDepthsSpell>();
            content.AddSpell<DesertRiteSpell>();
            content.AddSpell<EyesOfProfitSpell>();
            content.AddSpell<GlassCannonSpell>();
            content.AddSpell<ObsidianSkinSpell>();
            content.AddSpell<ShadowStepSpell>();
            content.AddSpell<VoidMarkSpell>();

            // Level 6
            content.AddSpell<DungeonGateSpell>();
            content.AddSpell<FortressStanceSpell>();
            content.AddSpell<PulseHealingSpell>();
            content.AddSpell<RitualOfHarvestSpell>();
            content.AddSpell<SelfDefenseHexSpell>();
            content.AddSpell<ShapedChargeSpell>();
            content.AddSpell<SubspaceSpell>();

            Add(Type, content);
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.value = Item.buyPrice(0, 0, 0, 1);
            //Item.value = Item.buyPrice(0, 30);
            Item.rare = ItemRarityID.LightRed;
        }
    }
}