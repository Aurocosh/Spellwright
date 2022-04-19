using Microsoft.Xna.Framework;
using Spellwright.Content.Spells.Base.CostModifiers;
using Spellwright.Content.Spells.Base.Description;
using Spellwright.Content.Spells.Base.Modifiers;
using Spellwright.Content.Spells.Base.SpellCosts;
using Spellwright.Core.Spells;
using Spellwright.Extensions;
using Spellwright.Util;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Spellwright.Content.Spells.Base
{
    public abstract class ModSpell : ModType
    {
        public int Type { get; private set; }

        public ModTranslation DisplayName { get; internal set; }
        public ModTranslation Description { get; internal set; }

        protected int guaranteedUses;
        protected float stability;
        protected int damage;
        protected float knockback;
        protected DamageClass damageType;
        protected bool canAutoReuse;
        protected int useDelay;
        protected SpellCost spellCost;
        protected float useTimeMultiplier;
        protected LegacySoundStyle useSound;
        protected float costModifier;
        public virtual int SpellLevel { get; protected set; }
        public SpellType UseType { get; protected set; }

        private SpellModifier appplicableModifiers;
        private readonly Dictionary<SpellModifier, ICostModifier> spellCostModifiers;

        public bool IsModifiersApplicable(SpellModifier spellModifiers) => spellModifiers == SpellModifier.None || spellModifiers.MissesAny(appplicableModifiers);
        public virtual bool CanAutoReuse(int playerLevel) => canAutoReuse;
        public virtual float GetUseSpeedMultiplier(int playerLevel) => useTimeMultiplier;
        public virtual int GetGuaranteedUses(int playerLevel) => guaranteedUses;
        public virtual float GetStability(int playerLevel) => stability;
        public virtual int GetUseDelay(int playerLevel) => useDelay;
        protected virtual int GetDamage(int playerLevel) => damage;
        protected virtual float GetKnockback(int playerLevel) => knockback;
        protected virtual DamageClass DamageType => damageType;
        protected virtual void PlayUseSound(Vector2 position)
        {
            if (useSound != null)
                SoundEngine.PlaySound(useSound, position);
        }

        public virtual List<SpellParameter> GetDescriptionValues(Player player, int playerLevel, SpellData spellData, bool fullVersion)
        {
            var values = new List<SpellParameter>();
            values.Add(new SpellParameter("SpellLevel", SpellLevel.ToString()));

            if (fullVersion)
            {
                string useTypeLocal = Spellwright.GetTranslation("SpellTypes", UseType.ToString()).Value;
                values.Add(new SpellParameter("SpellType", useTypeLocal));

                if (spellCost != null)
                {
                    string costDescription = spellCost.GetDescription(player, playerLevel, spellData);
                    values.Add(new SpellParameter("UseCost", costDescription));
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

        protected void AddApplicableModifier(SpellModifier spellModifier) => appplicableModifiers = appplicableModifiers.Add(spellModifier);

        protected void RemoveApplicableModifier(SpellModifier spellModifier) => appplicableModifiers = appplicableModifiers.Remove(spellModifier);
        protected void SetSpellCostModifier(SpellModifier spellModifier, ICostModifier costModifier) => spellCostModifiers[spellModifier] = costModifier;

        protected ModSpell()
        {
            UseType = SpellType.Invocation;

            guaranteedUses = 0;
            stability = 0;
            canAutoReuse = false;
            useDelay = 120;
            damage = 0;
            knockback = 0;
            spellCost = null;
            damageType = DamageClass.Generic;
            costModifier = 1f;
            appplicableModifiers = SpellModifier.None;
            spellCostModifiers = new Dictionary<SpellModifier, ICostModifier>();

            SetSpellCostModifier(SpellModifier.IsDispel, new MultCostModifier(0));
            SetSpellCostModifier(SpellModifier.IsAoe, new MultCostModifier(4));
            SetSpellCostModifier(SpellModifier.IsSelfless, new MultCostModifier(.75f));
            SetSpellCostModifier(SpellModifier.IsEternal, new MultCostModifier(4));
            SetSpellCostModifier(SpellModifier.IsTwofold, new MultCostModifier(2));
            SetSpellCostModifier(SpellModifier.IsFivefold, new MultCostModifier(5));
            SetSpellCostModifier(SpellModifier.IsTenfold, new MultCostModifier(10));
            SetSpellCostModifier(SpellModifier.IsFiftyfold, new MultCostModifier(50));
        }
        protected sealed override void Register()
        {
            ModTypeLookup<ModSpell>.Register(this);

            Type = SpellLoader.RegisterSpell(this);

            var nameKey = Spellwright.GetTranslationKey("Spells", Name, "Name");
            var descriptionKey = Spellwright.GetTranslationKey("Spells", Name, "Description");

            DisplayName = UtilLang.GetOrCreateTranslation(nameKey);
            Description = UtilLang.GetOrCreateTranslation(descriptionKey);

            SpellLibrary.RegisterSpell(this);
        }
        public sealed override void SetupContent()
        {
            SetStaticDefaults();
        }

        public override void SetStaticDefaults() { }

        public float GetCostModifier(SpellModifier spellModifiers)
        {
            float actualCostModifier = costModifier;
            foreach (var modifier in spellModifiers.SplitValues<SpellModifier>())
                if (spellCostModifiers.TryGetValue(modifier, out var spellCostModifier))
                    actualCostModifier = spellCostModifier.ModifyCost(actualCostModifier);
            return actualCostModifier;
        }

        public virtual bool ConsumeReagents(Player player, int playerLevel, SpellData spellData)
        {
            if (spellCost == null)
                return true;

            bool success = spellCost.Consume(player, playerLevel, spellData);
            if (!success)
            {
                Main.NewText(spellCost.LastError, spellCost.ErrorColor);
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

        public virtual bool ProcessExtraData(SpellStructure structure, out object extraData)
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
