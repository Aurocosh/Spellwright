using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Projectiles.Explosive;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Modifiers;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Content.Spells.Base.Types;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.Explosive
{
    internal class DragonSpitSpell : ProjectileSpell
    {
        public DragonSpitSpell()
        {
            AddApplicableModifier(ModifierConstants.UsebleModifiers);

            UnlockCost = new MultipleItemSpellCost()
                .WithCost(ItemID.Fireblossom, 10)
                .WithCost(ItemID.Bomb, 30);
        }

        public override int GetGuaranteedUses(int playerLevel) => 12 + 3 * playerLevel;

        public override void SetStaticDefaults()
        {
            SpellLevel = 2;
            UseType = SpellType.Spell;

            stability = .5f;

            damage = 1;
            knockback = 8f;
            damageType = DamageClass.Magic;
            projectileType = ModContent.ProjectileType<DragonSpitProjectile>();
            projectileSpeed = 10;
            canAutoReuse = false;
            useTimeMultiplier = 3f;

            UnlockCost = new SingleItemSpellCost(ItemID.Bomb, 20);
            SpellCost = new SingleItemSpellCost(ModContent.ItemType<RareSpellReagent>(), 1);
        }
    }
}
