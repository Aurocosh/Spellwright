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
            var content = new SpellTomeContent();
            content.AddCount(2, .70);
            content.AddCount(3, .25);
            content.AddCount(4, .05);

            // Level 4
            content.AddSpell<BewitchSpell>();
            content.AddSpell<BlockSpitterSpell>(2);
            content.AddSpell<BloodArrowSpell>();
            content.AddSpell<BoltOfConfusionSpell>();
            content.AddSpell<HellGateSpell>(3);
            content.AddSpell<HolyShoesSpell>(2);
            content.AddSpell<PiggySpell>(3);
            content.AddSpell<PotionVoidSpell>();
            content.AddSpell<RainCallSpell>();
            content.AddSpell<SkyGateSpell>(2);
            content.AddSpell<TileRollerSpell>();

            // Level 5
            content.AddSpell<BurningSoulSpell>();
            content.AddSpell<CallOfTheDepthsSpell>(3);
            content.AddSpell<ClearMindSpell>();
            content.AddSpell<CycleOfEternitySpell>();
            content.AddSpell<DesertRiteSpell>();
            content.AddSpell<EyesOfProfitSpell>(3);
            content.AddSpell<GlassCannonSpell>();
            content.AddSpell<ObsidianSkinSpell>(2);
            content.AddSpell<ShadowStepSpell>(2);
            content.AddSpell<VoidMarkSpell>(2);

            // Level 6
            content.AddSpell<DungeonGateSpell>(3);
            content.AddSpell<FortressStanceSpell>();
            content.AddSpell<ItemVoidSpell>(3);
            content.AddSpell<PulseHealingSpell>();
            content.AddSpell<RitualOfHarvestSpell>(2);
            content.AddSpell<SelfDefenseHexSpell>();
            content.AddSpell<ShapedChargeSpell>(2);

            Add(Type, content);
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.value = Item.buyPrice(0, 1);
            Item.rare = ItemRarityID.LightRed;
        }
    }
}