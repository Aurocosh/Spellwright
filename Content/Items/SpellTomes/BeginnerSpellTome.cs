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
            content.AddSpell<MysticSenseSpell>();
            content.AddSpell<RecallSpell>();
            content.AddSpell<HomeReflectionSpell>();

            // Level 1
            content.AddSpell<WillOfTorchGodSpell>();
            content.AddSpell<ConjureWaterSpell>();
            content.AddSpell<ReturnToFishSpell>();
            content.AddSpell<FireballSpell>();
            content.AddSpell<KissOfCloverSpell>();
            content.AddSpell<TorchMufflerSpell>();
            content.AddSpell<SparkCasterSpell>();
            content.AddSpell<PremonitionSpell>();

            // Level 2
            content.AddSpell<WarpMirrorSpell>();
            content.AddSpell<BattlecrySpell>();
            content.AddSpell<StitchWoundsSpell>();
            content.AddSpell<StoneBulletSpell>();
            content.AddSpell<FlashStepSpell>();
            content.AddSpell<InnerSunshineSpell>();
            content.AddSpell<ShellOfIceSpell>();
            content.AddSpell<IceBreakerSpell>();
            content.AddSpell<TigerEyesSpell>();
            content.AddSpell<DragonSpitSpell>();

            // Level 3
            content.AddSpell<OceanGateSpell>();
            content.AddSpell<SurgeOfLifeSpell>();
            content.AddSpell<AirDashSpell>();
            content.AddSpell<ManaStarfallSpell>();
            content.AddSpell<GaleForceSpell>();
            content.AddSpell<EvaporateSpell>();
            content.AddSpell<FanOfFlamesSpell>();
            content.AddSpell<BirdOfMidasSpell>();
            content.AddSpell<HeartThrowerSpell>();

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