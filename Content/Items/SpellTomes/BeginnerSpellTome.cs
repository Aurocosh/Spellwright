using Spellwright.Content.Items.SpellTomes.Base;
using Spellwright.Content.Spells.BuffSpells;
using Spellwright.Content.Spells.BuffSpells.Defensive;
using Spellwright.Content.Spells.BuffSpells.Sigils;
using Spellwright.Content.Spells.BuffSpells.Utility;
using Spellwright.Content.Spells.BuffSpells.Vanilla;
using Spellwright.Content.Spells.Enchant;
using Spellwright.Content.Spells.Explosive;
using Spellwright.Content.Spells.Healing;
using Spellwright.Content.Spells.Herbs;
using Spellwright.Content.Spells.Items;
using Spellwright.Content.Spells.LiquidSpawn;
using Spellwright.Content.Spells.Minions;
using Spellwright.Content.Spells.Movement;
using Spellwright.Content.Spells.Other;
using Spellwright.Content.Spells.Projectiles;
using Spellwright.Content.Spells.SpellRelated;
using Spellwright.Content.Spells.Storage;
using Spellwright.Content.Spells.TileBreak;
using Spellwright.Content.Spells.TileSpawn;
using Spellwright.Content.Spells.Warp;
using Spellwright.Content.Spells.WorldEvents;
using Terraria;
using Terraria.ID;

namespace Spellwright.Content.Items.SpellTomes
{
    public class BeginnerSpellTome : SpellTome
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Beginner Spell Tome");
            Tooltip.SetDefault("A book that contains multitude of common low level spells. This tome contains spells ranging from level 1 to level 3.");

            var content = new SpellTomeContent();
            content.AddCount(2, .70);
            content.AddCount(3, .25);
            content.AddCount(4, .05);

            // Level 0
            content.AddSpell<AscendSpell>();
            content.AddSpell<MendSpell>();
            content.AddSpell<ReturnSpell>();
            content.AddSpell<SparkCasterSpell>();

            // Level 1
            content.AddSpell<FireballSpell>(3);
            content.AddSpell<ForceOfCreationSpell>(2);
            content.AddSpell<KissOfCloverSpell>();
            content.AddSpell<MineralFeverSpell>(3);
            content.AddSpell<NightEyeSpell>(2);
            content.AddSpell<PremonitionSpell>();
            content.AddSpell<ReturnToFishSpell>(2);
            content.AddSpell<SpringDropletSpell>();
            content.AddSpell<TorchEaterSpell>();
            content.AddSpell<WallShredderSpell>(2);
            content.AddSpell<WillOfTorchGodSpell>(3);

            // Level 2
            content.AddSpell<BattlecrySpell>();
            content.AddSpell<DragonSpitSpell>(2);
            content.AddSpell<FlashStepSpell>(3);
            content.AddSpell<InnerSunshineSpell>(3);
            content.AddSpell<ReagentVoidSpell>(2);
            content.AddSpell<SeaBlessingSpell>();
            content.AddSpell<SharpenSpell>();
            content.AddSpell<ShellOfIceSpell>();
            content.AddSpell<ShockwaveSpell>(2);
            //content.AddSpell<StoneBulletSpell>();
            content.AddSpell<TigerEyesSpell>();
            content.AddSpell<WallSpitterSpell>(2);

            // Level 3
            content.AddSpell<AirDashSpell>(3);
            content.AddSpell<BirdOfMidasSpell>(2);
            content.AddSpell<EvaporateSpell>();
            //content.AddSpell<FanOfFlamesSpell>();
            content.AddSpell<FeatherfallSpell>();
            content.AddSpell<GaleForceSpell>(2);
            content.AddSpell<HeartThrowerSpell>();
            content.AddSpell<ManaStarfallSpell>();
            content.AddSpell<OceanGateSpell>(3);
            content.AddSpell<RestockSpell>();
            content.AddSpell<SurgeOfLifeSpell>(2);
            content.AddSpell<WarpMirrorSpell>();

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

            // Level 7
            content.AddSpell<ManaShieldSpell>();
            content.AddSpell<PurifySpell>();
            content.AddSpell<SigilOfBerserkerSpell>(3);
            content.AddSpell<SigilOfLegionSpell>(3);
            content.AddSpell<SigilOfSageSpell>(3);
            content.AddSpell<SigilOfSniperSpell>(3);
            //content.AddSpell<SoulNibblerSpell>();

            // Level 8
            content.AddSpell<GravityDashSpell>(3);
            content.AddSpell<LavaSplashSpell>();
            content.AddSpell<ReactiveArmorSpell>();
            content.AddSpell<StateLockSpell>(2);
            content.AddSpell<VortexHandsSpell>();

            // Level 9
            content.AddSpell<AdventOfSummerSpell>(3);
            content.AddSpell<BindMirrorSpell>(2);
            content.AddSpell<GreedyVortexSpell>();

            // Level 10
            content.AddSpell<HymnOfDiscordSpell>(3);
            content.AddSpell<MagickaFairySpell>();
            content.AddSpell<MetabolicBoostSpell>(2);

            Add(Type, content);
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            //Item.value = Item.buyPrice(0, 0, 0, 1);
            Item.value = Item.buyPrice(0, 0, 20);
            Item.rare = ItemRarityID.Blue;
        }
    }
}