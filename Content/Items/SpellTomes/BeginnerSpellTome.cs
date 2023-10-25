using Spellwright.Content.Items.SpellTomes.Base;
using Spellwright.Content.Spells.BuffSpells;
using Spellwright.Content.Spells.BuffSpells.Utility;
using Spellwright.Content.Spells.BuffSpells.Vanilla;
using Spellwright.Content.Spells.Enchant;
using Spellwright.Content.Spells.Explosive;
using Spellwright.Content.Spells.LiquidSpawn;
using Spellwright.Content.Spells.Minions;
using Spellwright.Content.Spells.Movement;
using Spellwright.Content.Spells.Projectiles;
using Spellwright.Content.Spells.Storage;
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
            // DisplayName.SetDefault("Beginner Spell Tome");
            // Tooltip.SetDefault("A book that contains multitude of common low level spells. This tome contains spells ranging from level 1 to level 3.");

            var content = new SpellTomeContent();
            content.AddCount(2, .70);
            content.AddCount(3, .25);
            content.AddCount(4, .05);

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