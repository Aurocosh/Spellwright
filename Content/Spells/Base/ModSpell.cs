using Microsoft.Xna.Framework;
using Spellwright.Core.Spells;
using Spellwright.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Localization;
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
        protected int reagentType;
        protected int reagentUseCost;
        protected float useTimeMultiplier;
        public LegacySoundStyle useSound;
        public virtual int SpellLevel => 0;
        public SpellType UseType { get; protected set; }

        private readonly HashSet<SpellModifier> appplicableModifiers;
        private readonly Dictionary<SpellModifier, int> extraReagentCosts;

        public bool IsModifiersApplicable(IEnumerable<SpellModifier> spellModifiers) => appplicableModifiers.IsSupersetOf(spellModifiers);
        public virtual bool CanAutoReuse(int playerLevel) => canAutoReuse;
        public virtual float GetUseSpeedMultiplier(int playerLevel) => useTimeMultiplier;
        public virtual int GetGuaranteedUses(int playerLevel) => guaranteedUses;
        public virtual float GetStability(int playerLevel) => stability;
        public virtual int GetUseDelay(int playerLevel) => useDelay;
        protected virtual int GetDamage(int playerLevel) => damage;
        protected virtual float GetKnockback(int playerLevel) => knockback;
        protected virtual LegacySoundStyle GetUseSound(int playerLevel) => useSound;
        protected virtual DamageClass DamageType => damageType;

        protected void AddApplicableModifier(SpellModifier spellModifier) => appplicableModifiers.Add(spellModifier);
        protected void RemoveApplicableModifier(SpellModifier spellModifier) => appplicableModifiers.Remove(spellModifier);
        protected void SetExtraReagentCost(SpellModifier spellModifier, int amount) => extraReagentCosts[spellModifier] = amount;

        protected ModSpell()
        {
            UseType = SpellType.Invocation;

            guaranteedUses = 0;
            stability = 0;
            useDelay = 120;
            damage = 0;
            knockback = 0;
            reagentType = -1;
            reagentUseCost = 0;
            damageType = DamageClass.Generic;
            appplicableModifiers = new HashSet<SpellModifier>();
            extraReagentCosts = new Dictionary<SpellModifier, int>();
        }
        protected sealed override void Register()
        {
            ModTypeLookup<ModSpell>.Register(this);

            Type = SpellLoader.RegisterSpell(this);

            var nameKey = Spellwright.GetTranslationKey("Spells", Name, "Name");
            var descriptionKey = Spellwright.GetTranslationKey("Spells", Name, "Description");
            var incantationKey = Spellwright.GetTranslationKey("Spells", Name, "Incantation");

            DisplayName = LocalizationLoader.CreateTranslation(nameKey);
            Description = LocalizationLoader.CreateTranslation(descriptionKey);

            var localIncantation = Language.GetText(incantationKey).Value;
            if (!localIncantation.StartsWith("Mods.Spellwright"))
                SpellLibrary.SetSpellIncantation(localIncantation, this);

            var defaultIncantation = GetDefaultIncantation();
            if (defaultIncantation.ToLower() != localIncantation.ToLower())
                SpellLibrary.SetSpellIncantation(defaultIncantation, this);
        }
        public sealed override void SetupContent()
        {
            SetStaticDefaults();
        }

        public override void SetStaticDefaults() { }

        private string GetDefaultIncantation()
        {
            var name = Name;
            if (name.EndsWith("Spell"))
                name = name.Substring(0, name.LastIndexOf("Spell"));

            var builder = new StringBuilder();
            foreach (char c in name)
            {
                if (char.IsUpper(c) && builder.Length > 0)
                    builder.Append(' ');
                builder.Append(c);
            }
            return builder.ToString();
        }

        public virtual bool ConsumeReagents(Player player, int playerLevel, SpellData spellData)
        {
            if (reagentType <= 0)
                return true;

            int useCost = reagentUseCost;
            foreach (var modifier in spellData.GetModifiers())
                if (extraReagentCosts.TryGetValue(modifier, out var extraCost))
                    useCost += extraCost;

            if (useCost == 0)
                return true;

            if (!player.ConsumeItems(reagentType, useCost))
            {
                string message = Spellwright.GetTranslation("Messages", "NotEnoughReagents");
                Main.NewText(message, new Color(255, 140, 40, 255));
                return false;
            }
            return true;
        }

        public virtual bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            throw new NotImplementedException();
        }
        public virtual bool Cast(Player player, int playerLevel, SpellData spellData, IProjectileSource source, Vector2 position, Vector2 direction)
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
            var modifierIds = spellData.GetModifiers().Select(x => (int)x).ToList();
            var extrasDataTag = new TagCompound();
            SerializeExtraData(extrasDataTag, spellData.ExtraData);

            var tag = new TagCompound();
            tag.Add("SpellName", Name);
            tag.Add("Argument", spellData.Argument);
            tag.Add("ModifierList", modifierIds);
            tag.Add("ExtraData", extrasDataTag);
            return tag;
        }

        public SpellData DeserializeData(TagCompound tag)
        {
            var dataSpellName = tag.GetString("SpellName");
            var argument = tag.GetString("Argument");
            var modifierIds = tag.GetList<int>("ModifierList");
            var extraDataTag = tag.GetCompound("ExtraData");

            var modifiers = modifierIds.Select(x => (SpellModifier)x);

            object extraSpellData = null;
            if (dataSpellName == Name)
                extraSpellData = DeserializeExtraData(extraDataTag);

            return new SpellData(modifiers, argument, extraSpellData);
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
