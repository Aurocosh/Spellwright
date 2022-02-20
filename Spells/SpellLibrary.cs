using Spellwright.Spells.Base;
using Spellwright.Spells.BuffSpells;
using Spellwright.Spells.Explosive;
using Spellwright.Spells.Healing;
using Spellwright.Spells.WarpSpells;
using System;
using System.Collections.Generic;
using Terraria.ID;

namespace Spellwright.Spells
{
    internal class SpellLibrary
    {
        private readonly Dictionary<string, Spell> _spellNameMap = new();
        private readonly Dictionary<string, Spell> _spellIncantationMap = new();
        public SpellLibrary()
        {
            Init();
        }

        public Spell GetSpellByName(string name)
        {
            if (!_spellNameMap.TryGetValue(name.ToLower(), out Spell spell))
                return null;
            return spell;
        }

        public Spell GetSpellByIncantation(string incantation)
        {
            if (!_spellIncantationMap.TryGetValue(incantation.ToLower(), out Spell spell))
                return null;
            return spell;
        }

        public void RegisterSpell(Spell spell)
        {
            if (_spellNameMap.ContainsKey(spell.InternalName))
                throw new Exception("Name conflict");
            if (_spellIncantationMap.ContainsKey(spell.Incantation))
                throw new Exception("Incantation conflict");

            _spellNameMap.Add(spell.InternalName.ToLower(), spell);
            _spellIncantationMap.Add(spell.Incantation.ToLower(), spell);
        }

        private void Init()
        {
            //
            RegisterSpell(new MaterialShellSpell("BarkShell", "Bark shell", 3, TileID.BorealWood));
            RegisterSpell(new MaterialShellSpell("EarthShell", "Earth shell", 5, TileID.Dirt));
            RegisterSpell(new PurifySpell("Purify", "Purify"));

            // Projectiles
            RegisterSpell(new StoneBulletSpell("StoneBullet", "Stone bullet"));
            RegisterSpell(new FireBallSpell("Fireball", "Fireball"));
            RegisterSpell(new FanOfFlamesSpell("FanOfFlames", "Fan of flames"));
            RegisterSpell(new BloodArrowSpell("BloodArrow", "Blood arrow"));

            // Explosive
            RegisterSpell(new DragonSpitSpell("DragonSpit", "Dragon spit"));
            RegisterSpell(new BlastPebbleSpell("BlastPebble", "Blast pebble"));

            // Buffs
            RegisterSpell(new InnerSunshineSpell("InnerSunshine", "Inner sunshine"));
            RegisterSpell(new ReturnToFishSpell("ReturnToFish", "Return to fish"));
            RegisterSpell(new BloodyRageSpell("BloodyRage", "Bloody rage"));
            RegisterSpell(new TigerEyesSpell("TigerEyes", "Tiger eyes"));
            RegisterSpell(new BattlecrySpell("Battlecry", "Battlecry"));
            RegisterSpell(new ReactiveArmorSpell("ReactiveArmor", "Reactive armor"));
            RegisterSpell(new GreedyVortexSpell("GreedyVortex", "Greedy vortex"));
            RegisterSpell(new ManaStarfallSpell("ManaStarfall", "Mana starfall"));

            // Teleportation
            RegisterSpell(new HomeReflectionSpell("HomeReflection", "Home reflection"));
            RegisterSpell(new OceanGateSpell("OceanGate", "Ocean gate"));
            RegisterSpell(new BaseJumpSpell("BaseJump", "Base jump"));
            RegisterSpell(new HellGateSpell("HellGate", "Hell gate"));

            // Cantrips
            RegisterSpell(new BoltOfConfusionSpell("BoltOfConfusion", "Bolt of confusion"));
            RegisterSpell(new SparkCasterSpell("SparkCaster", "Spark caster"));

            // Healing
            RegisterSpell(new StitchWoundsSpell("StitchWounds", "Stitch wounds"));
            RegisterSpell(new SurgeOfLifeSpell("SurgeOfLife", "Surge of life"));

            // Movement
            RegisterSpell(new FlashStepSpell("FlashStep", "Flash step"));
            RegisterSpell(new AirDashSpell("AirDash", "Air dash"));

            // Liquid
            RegisterSpell(new ConjureWaterSpell("ConjureWater", "Conjure water"));
            RegisterSpell(new ConjureLavaSpell("ConjureLava", "Conjure lava"));

            // Items
            RegisterSpell(new VortexHandsSpell("VortexHands", "Vortex hands"));

            // Item spawn
            RegisterSpell(new BindMirrorSpell("BindMirror", "Bind mirror"));
            RegisterSpell(new WarpMirrorSpell("WarpMirror", "Warp mirror"));

            // Minions
            RegisterSpell(new BirdOfMidasSpell("BirdOfMidas", "Bird of midas"));

            // Tile spawn
            RegisterSpell(new WillOfTorchGodSpell("WillOfTorchGod", "Will of Torch god"));
            RegisterSpell(new ShellOfIceSpell("ShellOfIce", "Shell of ice"));

            // Tile break
            RegisterSpell(new IceBreakerSpell("IceBreaker", "Ice breaker"));
            RegisterSpell(new SnuffOutSpell("SnuffOut", "Snuff out"));

        }
    }
}
