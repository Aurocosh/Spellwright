using Microsoft.Xna.Framework;
using Spellwright.Extensions;
using Spellwright.Spells.Base;
using Spellwright.Spells.SpellExtraData;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Spellwright.Spells
{
    internal abstract class Spell
    {
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
        public string InternalName { get; }
        public string Incantation { get; }
        public SpellType Type { get; }

        private readonly HashSet<SpellModifier> appplicableModifiers;
        private readonly Dictionary<SpellModifier, int> extraReagentCosts;

        public string Name => Spellwright.GetTranslation("Spells", InternalName, "Name");
        public string Description => Spellwright.GetTranslation("Spells", InternalName, "Description");
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

        protected Spell(string name, string incantation, SpellType type)
        {
            InternalName = name;
            guaranteedUses = 0;
            stability = 0;
            Incantation = incantation;
            Type = type;
            useDelay = 120;
            damage = 0;
            knockback = 0;
            reagentType = -1;
            reagentUseCost = 0;
            damageType = DamageClass.Generic;
            appplicableModifiers = new HashSet<SpellModifier>();
            extraReagentCosts = new Dictionary<SpellModifier, int>();
        }

        public virtual bool ConsumeReagents(Player player, int playerLevel, SpellData spellData)
        {
            if (reagentType <= 0)
                return true;

            int useCost = reagentUseCost;
            foreach (var modifier in spellData.GetModifiers())
            {
                if (extraReagentCosts.TryGetValue(modifier, out var extraCost))
                    useCost += extraCost;
            }

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

        public virtual bool ProcessExtraData(SpellStructure spellStructure, out SpellData spellData)
        {
            spellData = new SpellData(spellStructure);
            return true;
        }
    }
}
