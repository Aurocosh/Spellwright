using Spellwright.Content.Items.SpellTomes.Base;
using Spellwright.Content.Spells.BuffSpells;
using Spellwright.Content.Spells.BuffSpells.Utility;
using Spellwright.Content.Spells.BuffSpells.Vanilla;
using Spellwright.Content.Spells.Enchant;
using Spellwright.Content.Spells.Explosive;
using Spellwright.Content.Spells.Healing;
using Spellwright.Content.Spells.LiquidSpawn;
using Spellwright.Content.Spells.Minions;
using Spellwright.Content.Spells.Movement;
using Spellwright.Content.Spells.Projectiles;
using Spellwright.Content.Spells.SpellRelated;
using Spellwright.Content.Spells.TileBreak;
using Spellwright.Content.Spells.TileSpawn;
using Spellwright.Content.Spells.Warp;
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

            // Level 0
            content.AddSpell<AscendSpell>();
            content.AddSpell<HomeReflectionSpell>();
            content.AddSpell<MysticSenseSpell>();
            content.AddSpell<RecallSpell>();

            // Level 1
            content.AddSpell<ConjureWaterSpell>();
            content.AddSpell<FireballSpell>();
            content.AddSpell<KissOfCloverSpell>();
            content.AddSpell<PremonitionSpell>();
            content.AddSpell<ReturnToFishSpell>();
            content.AddSpell<SparkCasterSpell>();
            content.AddSpell<TorchMufflerSpell>();
            content.AddSpell<WillOfTorchGodSpell>();

            // Level 2
            content.AddSpell<BattlecrySpell>();
            content.AddSpell<DragonSpitSpell>();
            content.AddSpell<FlashStepSpell>();
            content.AddSpell<IceBreakerSpell>();
            content.AddSpell<InnerSunshineSpell>();
            content.AddSpell<ShellOfIceSpell>();
            content.AddSpell<StitchWoundsSpell>();
            content.AddSpell<StoneBulletSpell>();
            content.AddSpell<TigerEyesSpell>();
            content.AddSpell<WarpMirrorSpell>();

            // Level 3
            content.AddSpell<AirDashSpell>();
            content.AddSpell<BirdOfMidasSpell>();
            content.AddSpell<EvaporateSpell>();
            content.AddSpell<FanOfFlamesSpell>();
            content.AddSpell<GaleForceSpell>();
            content.AddSpell<HeartThrowerSpell>();
            content.AddSpell<ManaStarfallSpell>();
            content.AddSpell<OceanGateSpell>();
            content.AddSpell<SurgeOfLifeSpell>();

            Add(Type, content);
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.value = Item.buyPrice(0, 0, 0, 1);
            //Item.value = Item.buyPrice(0, 0, 30);
            Item.rare = ItemRarityID.Blue;
        }
    }
}