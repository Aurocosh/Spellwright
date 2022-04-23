using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Content.Spells.Base.Types;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.Projectiles
{
    internal class MoneyTroughSpell : ProjectileSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 4;
            UseType = SpellType.Invocation;
            projectileType = ProjectileID.FlyingPiggyBank;
            useSound = SoundID.Item59;
            canAutoReuse = false;
            useTimeMultiplier = 1f;

            UnlockCost = new SingleItemSpellCost(ItemID.PiggyBank);
            SpellCost = new ReagentSpellCost(ModContent.ItemType<CommonSpellReagent>(), 1);
        }
    }
}
