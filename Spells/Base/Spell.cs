using Microsoft.Xna.Framework;
using Spellwright.Spells.Base;
using System;
using Terraria;
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
        protected float useTimeMultiplier;
        public virtual int SpellLevel => 0;
        public string InternalName { get; }
        public string Incantation { get; }
        public SpellType Type { get; }

        public string Name => Spellwright.GetTranslation("Spells", InternalName, "Name");
        public string Description => Spellwright.GetTranslation("Spells", InternalName, "Description");

        public virtual bool CanAutoReuse(int playerLevel) => canAutoReuse;
        public virtual float GetUseSpeedMultiplier(int playerLevel) => useTimeMultiplier;
        public virtual int GetGuaranteedUses(int playerLevel) => guaranteedUses;
        public virtual float GetStability(int playerLevel) => stability;
        public virtual int GetUseDelay(int playerLevel) => useDelay;
        protected virtual int GetDamage(int playerLevel) => damage;
        protected virtual float GetKnockback(int playerLevel) => knockback;
        protected virtual DamageClass DamageType => damageType;

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
            damageType = DamageClass.Generic;
        }

        public virtual bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            throw new NotImplementedException();
        }
        public virtual bool Cast(Player player, int playerLevel, SpellData spellData, IProjectileSource source, Vector2 position, Vector2 direction)
        {
            throw new NotImplementedException();
        }

        public virtual bool ProcessExtraData(string argument, out SpellData spellData)
        {
            spellData = null;
            return true;
        }
    }
}
