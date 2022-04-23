using Microsoft.Xna.Framework;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Projectiles.Tiles;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.TileSpawn
{
    internal class WallSpitterSpell : ProjectileSpell
    {
        public override int GetGuaranteedUses(int playerLevel) => 30 + 5 * playerLevel;

        public override void SetStaticDefaults()
        {
            SpellLevel = 4;
            UseType = SpellType.Spell;

            stability = .2f;

            damage = 1;
            knockback = 8f;
            damageType = DamageClass.Magic;
            projectileType = ModContent.ProjectileType<WallSpitterProjectile>();
            projectileSpeed = 10;
            canAutoReuse = false;
            useTimeMultiplier = 9f;

            UnlockCost = new MultipleItemSpellCost()
                .WithCost(ItemID.GrayBrickWall, 100)
                .WithCost(ItemID.Bomb, 10);

            SpellCost = new SingleItemSpellCost(ModContent.ItemType<RareSpellReagent>(), 5);
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData, IEntitySource source, Vector2 position, Vector2 direction)
        {
            if (ActivateProjectiles(player))
                return false;
            if (WallSpitterProjectile.GetValidItem(player) == null)
            {
                var error = Spellwright.GetTranslation("Spells", Name, "NoWallsInToolbar");
                Main.NewText(error, Color.Orange);
                return false;
            }

            return base.Cast(player, playerLevel, spellData, source, position, direction);
        }

        private bool ActivateProjectiles(Player player)
        {
            foreach (var projectile in UtilProjectiles.FindProjectiles(player.whoAmI, projectileType))
            {
                projectile.Kill();
                return true;
            }
            return false;
        }
    }
}
