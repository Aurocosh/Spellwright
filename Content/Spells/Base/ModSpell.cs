﻿using Microsoft.Xna.Framework;
using Spellwright.Config;
using Spellwright.Content.Spells.Base.CostModifiers;
using Spellwright.Content.Spells.Base.Description;
using Spellwright.Content.Spells.Base.Modifiers;
using Spellwright.Content.Spells.Base.SpellCosts;
using Spellwright.Core.Spells;
using Spellwright.ExecutablePackets.Broadcast.DustSpawners;
using Spellwright.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Spellwright.Content.Spells.Base
{
    public abstract class ModSpell : ModType
    {
        public int Type { get; private set; }

        public LocalizedText DisplayName { get; internal set; }
        public LocalizedText Description { get; internal set; }

        protected int guaranteedUses;
        protected float stability;
        protected int damage;
        protected float knockback;
        protected DamageClass damageType;
        protected bool canAutoReuse;
        protected int useDelay;
        protected float useTimeMultiplier;
        protected SoundStyle? castSound;
        protected SoundStyle? useSound;
        protected float costModifier;
        protected bool costIsImportant;
        private SpellCost castCost;
        private SpellCost useCost;
        private SpellCost unlockCost;

        public virtual int SpellLevel { get; protected set; }
        public SpellType UseType { get; protected set; }
        public SpellCost CastCost
        {
            get => SpellwrightServerConfig.Instance.CastCostEnabled || costIsImportant ? castCost : null;
            protected set => castCost = value;
        }
        public SpellCost UseCost
        {
            get => SpellwrightServerConfig.Instance.CastCostEnabled || costIsImportant ? useCost : null;
            protected set => useCost = value;
        }
        public SpellCost UnlockCost
        {
            get => SpellwrightServerConfig.Instance.UnlockCostEnabled ? unlockCost : null;
            protected set => unlockCost = value;
        }
        public SpellModifier AppplicableModifiers { get; protected set; }
        private readonly Dictionary<SpellModifier, ICostModifier> spellCostModifiers;

        public bool IsModifiersApplicable(SpellModifier spellModifiers) => spellModifiers == SpellModifier.None || !AppplicableModifiers.MissesAny(spellModifiers);
        public virtual bool CanAutoReuse(int playerLevel) => canAutoReuse;
        public virtual float GetUseSpeedMultiplier(int playerLevel) => useTimeMultiplier;
        public virtual int GetGuaranteedUses(int playerLevel) => guaranteedUses;
        public virtual float GetStability(int playerLevel) => stability;
        public virtual int GetUseDelay(int playerLevel) => useDelay;
        protected virtual int GetDamage(int playerLevel) => damage;
        protected virtual float GetKnockback(int playerLevel) => knockback;
        protected virtual DamageClass DamageType => damageType;
        protected void AddApplicableModifier(SpellModifier spellModifier) => AppplicableModifiers = AppplicableModifiers.Add(spellModifier);
        protected void RemoveApplicableModifier(SpellModifier spellModifier) => AppplicableModifiers = AppplicableModifiers.Remove(spellModifier);
        protected void SetSpellCostModifier(SpellModifier spellModifier, ICostModifier costModifier) => spellCostModifiers[spellModifier] = costModifier;

        protected LocalizedText GetTranslation(string key) => Spellwright.GetTranslation("Spells", Name, key);

        protected ModSpell()
        {
            UseType = SpellType.Invocation;

            guaranteedUses = 0;
            stability = 0;
            canAutoReuse = false;
            useDelay = 120;
            damage = 0;
            knockback = 0;
            CastCost = null;
            damageType = DamageClass.Generic;
            costModifier = 1f;
            costIsImportant = false;
            AppplicableModifiers = SpellModifier.None;
            spellCostModifiers = new Dictionary<SpellModifier, ICostModifier>();
            castSound = SoundID.Item4;

            SetSpellCostModifier(SpellModifier.Dispel, new MultCostModifier(0));
            SetSpellCostModifier(SpellModifier.Area, new MultCostModifier(3));
            SetSpellCostModifier(SpellModifier.Eternal, new MultCostModifier(6));
        }
        protected sealed override void Register()
        {
            ModTypeLookup<ModSpell>.Register(this);

            Type = SpellLoader.RegisterSpell(this);

            var nameKey = Spellwright.GetTranslationKey("Spells", Name, "Name");
            var descriptionKey = Spellwright.GetTranslationKey("Spells", Name, "Description");

            DisplayName = Language.GetOrRegister(nameKey);
            Description = Language.GetOrRegister(descriptionKey);

            SpellLibrary.RegisterSpell(this);
        }
        public sealed override void SetupContent()
        {
            SetStaticDefaults();
        }

        public override void SetStaticDefaults() { }

        public virtual void PlayCastSound(Vector2 position)
        {
            if (castSound != null)
                SoundEngine.PlaySound(castSound.Value, position);
        }

        public virtual void PlayUseSound(Vector2 position)
        {
            if (useSound != null)
                SoundEngine.PlaySound(useSound.Value, position);
        }

        public virtual void DoCastEffect(Player player, int playerLevel)
        {
            var spawner = new CastDustSpawner(player, SpellLevel);
            spawner.Execute();
        }

        public virtual List<SpellParameter> GetDescriptionValues(Player player, int playerLevel, SpellData spellData, bool fullVersion)
        {
            var values = new List<SpellParameter>();
            values.Add(new SpellParameter("SpellLevel", SpellLevel.ToString()));

            if (fullVersion)
            {
                string useTypeLocal = Spellwright.GetTranslation("SpellTypes", UseType.ToString()).Value;
                values.Add(new SpellParameter("SpellType", useTypeLocal));

                var incantations = SpellLibrary.GetSpellIncantationList(Type).Select(x => x.FirstCharToUpper());
                var incantationPart = string.Join(", ", incantations);
                values.Add(new SpellParameter("Incantation", incantationPart));

                if (CastCost != null)
                {
                    string costDescription = CastCost.GetDescription(player, playerLevel, spellData);
                    values.Add(new SpellParameter("UseCost", costDescription));
                }

                if (UseCost != null)
                {
                    string costDescription = UseCost.GetDescription(player, playerLevel, spellData);
                    values.Add(new SpellParameter("CastCost", costDescription));
                }

                if (AppplicableModifiers != SpellModifier.None)
                {
                    var modifierList = AppplicableModifiers.SplitValues<SpellModifier>().Select(x => Spellwright.GetTranslation("SpellModifiers", x.ToString()).Value).ToList();
                    var modifierPart = string.Join(", ", modifierList);
                    values.Add(new SpellParameter("ApplicableModifiers", modifierPart));
                }
            }

            float stability = GetStability(playerLevel);
            if (stability > 0)
                values.Add(new SpellParameter("Stability", $"{(int)(stability * 100)}%"));

            int damage = GetDamage(playerLevel);
            if (damage > 0)
                values.Add(new SpellParameter("Damage", damage.ToString()));

            return values;
        }

        public float GetCostModifier(SpellModifier spellModifiers)
        {
            float actualCostModifier = costModifier;
            foreach (var modifier in spellModifiers.SplitValues<SpellModifier>())
                if (spellCostModifiers.TryGetValue(modifier, out var spellCostModifier))
                    actualCostModifier = spellCostModifier.ModifyCost(actualCostModifier);
            return actualCostModifier;
        }

        public virtual bool ConsumeReagentsCast(Player player, int playerLevel, SpellData spellData)
        {
            if (CastCost == null)
                return true;

            bool success = CastCost.Consume(player, playerLevel, spellData);
            if (!success)
            {
                Main.NewText(CastCost.LastError, CastCost.ErrorColor);
                return false;
            }
            return true;
        }

        public virtual bool ConsumeReagentsUse(Player player, int playerLevel, SpellData spellData)
        {
            if (UseCost == null)
                return true;

            float spellStability = GetStability(playerLevel);
            if (spellStability > 0 && Main.rand.NextFloat() < spellStability)
                return true;

            bool success = UseCost.Consume(player, playerLevel, spellData);
            if (!success)
            {
                Main.NewText(UseCost.LastError, UseCost.ErrorColor);
                return false;
            }
            return true;
        }

        public virtual bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            throw new NotImplementedException();
        }
        public virtual bool Cast(Player player, int playerLevel, SpellData spellData, IEntitySource source, Vector2 position, Vector2 direction)
        {
            throw new NotImplementedException();
        }

        public virtual bool ProcessExtraData(Player player, SpellStructure structure, out object extraData)
        {
            extraData = null;
            return true;
        }

        public TagCompound SerializeData(SpellData spellData)
        {
            var extrasDataTag = new TagCompound();
            SerializeExtraData(extrasDataTag, spellData.ExtraData);

            var tag = new TagCompound();
            tag.Add("SpellName", Name);
            tag.Add("Argument", spellData.Argument);
            tag.Add("Modifiers", (int)spellData.SpellModifiers);
            tag.Add("ExtraData", extrasDataTag);
            return tag;
        }

        public SpellData DeserializeData(TagCompound tag)
        {
            var dataSpellName = tag.GetString("SpellName");
            var argument = tag.GetString("Argument");
            var modifiers = (SpellModifier)tag.GetInt("Modifiers");
            var extraDataTag = tag.GetCompound("ExtraData");

            var costModifier = GetCostModifier(modifiers);

            object extraSpellData = null;
            if (dataSpellName == Name)
                extraSpellData = DeserializeExtraData(extraDataTag);

            return new SpellData(modifiers, argument, costModifier, extraSpellData);
        }

        public virtual void SerializeExtraData(TagCompound tag, object extraData)
        {
        }

        public virtual object DeserializeExtraData(TagCompound tag)
        {
            return null;
        }
    }
}
