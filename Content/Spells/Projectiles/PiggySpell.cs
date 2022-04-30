using Microsoft.Xna.Framework;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Content.Spells.Base.SpellCosts.Reagent;
using Spellwright.Content.Spells.Base.Types;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.Projectiles
{
    internal class PiggySpell : ProjectileSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 4;
            UseType = SpellType.Invocation;
            projectileType = ProjectileID.FlyingPiggyBank;
            castSound = SoundID.Item59;
            canAutoReuse = false;
            useTimeMultiplier = 1f;

            UnlockCost = new SingleItemSpellCost(ItemID.PiggyBank);
            CastCost = new ReagentSpellCost(ModContent.ItemType<CommonSpellReagent>(), 1);
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            Vector2 velocity = Vector2.UnitX * player.direction;
            var source = new EntitySource_Parent(player);
            SpawnProjectile(player, playerLevel, spellData, source, player.Center, velocity);
            return true;
        }
    }
}
