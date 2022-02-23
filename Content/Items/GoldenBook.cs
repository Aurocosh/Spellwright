using Microsoft.Xna.Framework;
using Spellwright.Common.Players;
using Spellwright.Content.Spells.Base;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Spellwright.Content.Items
{
    public class GoldenBook : ModItem
    {
        public GoldenBook()
        {
        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("This is an example magic weapon");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.useTime = 100;
            Item.useAnimation = 100;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = false;
            Item.damage = 20;
            Item.DamageType = DamageClass.Magic;
            Item.width = 32;
            Item.height = 32;
            Item.shoot = ProjectileID.FireArrow;
            Item.useAmmo = AmmoID.None;
            Item.noMelee = true; // Makes the item not do damage with it's melee hitbox.
            Item.shootSpeed = 7; // How fast the item shoots the projectile.

            Item.value = 10000;
            Item.rare = 2;
        }

        public override float UseSpeedMultiplier(Player player)
        {
            SpellwrightPlayer spellwrightPlayer = Main.LocalPlayer.GetModPlayer<SpellwrightPlayer>();
            ModSpell spell = spellwrightPlayer.CurrentSpell;
            int playerLevel = spellwrightPlayer.PlayerLevel;
            return spell?.GetUseSpeedMultiplier(playerLevel) ?? 4f;
        }

        public override bool? CanAutoReuseItem(Player player)
        {
            SpellwrightPlayer spellwrightPlayer = Main.LocalPlayer.GetModPlayer<SpellwrightPlayer>();
            ModSpell spell = spellwrightPlayer.CurrentSpell;
            int playerLevel = spellwrightPlayer.PlayerLevel;
            return spell?.CanAutoReuse(playerLevel) ?? false;
        }

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SpellwrightPlayer spellwrightPlayer = Main.LocalPlayer.GetModPlayer<SpellwrightPlayer>();
            ModSpell spell = spellwrightPlayer.CurrentSpell;
            SpellData spellData = spellwrightPlayer.SpellData;
            int playerLevel = spellwrightPlayer.PlayerLevel;
            if (spell != null && spellData != null)
            {
                if (!spell.ConsumeReagents(player, playerLevel, spellData))
                    return false;

                bool canCast = false;
                bool consumeCharge = false;
                if (spellwrightPlayer.GuaranteedUsesLeft > 0)
                {
                    canCast = true;
                    consumeCharge = true;
                }
                else
                {
                    float stability = spell.GetStability(playerLevel);
                    if (stability > 0)
                    {
                        var randomRoll = Main.rand.NextDouble();
                        if (randomRoll < stability)
                            canCast = true;
                    }
                }

                if (canCast)
                {
                    bool success = spell.Cast(player, playerLevel, spellData, source, position, velocity);
                    if (success && consumeCharge)
                        spellwrightPlayer.GuaranteedUsesLeft--;
                }
                else
                {
                    spellwrightPlayer.CurrentSpell = null;
                    spellwrightPlayer.SpellData = null;
                    SoundEngine.PlaySound(SoundID.Item35, position);
                }
            }
            return false;
        }

        public override void AddRecipes()
        {
            //ModRecipe recipe = new ModRecipe(mod);
            //recipe.AddIngredient(ItemID.DirtBlock, 1);
            //recipe.AddTile(TileID.WorkBenches);
            //recipe.SetResult(this);
            //recipe.AddRecipe();
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            var nameTooltip = tooltips[0];
            nameTooltip.text += " Test";
            tooltips.Clear();
            tooltips.Add(nameTooltip);

            Spellwright spellwright = Spellwright.Instance;
            SpellwrightPlayer spellwrightPlayer = Main.LocalPlayer.GetModPlayer<SpellwrightPlayer>();
            ModSpell spell = spellwrightPlayer.CurrentSpell;
            int playerLevel = spellwrightPlayer.PlayerLevel;
            if (spell == null)
                tooltips.Add(new TooltipLine(spellwright, "", "You have no active spells"));
            else
            {
                if (spellwrightPlayer.GuaranteedUsesLeft > 0)
                    tooltips.Add(new TooltipLine(spellwright, "Spell uses", $"Spell uses left {spellwrightPlayer.GuaranteedUsesLeft}"));

                float stability = spell.GetStability(playerLevel);
                if (stability > 0)
                    tooltips.Add(new TooltipLine(spellwright, "Spell stability", $"Spell stability {(int)(stability * 100)}%"));

                tooltips.Add(new TooltipLine(spellwright, "Description", spell.Description.GetTranslation(Language.ActiveCulture)));
            }

            foreach (TooltipLine tooltip in tooltips)
                if (tooltip.mod.Equals("Terraria") && tooltip.Name.Equals("Tooltip2"))
                    tooltip.text = "SpellIds: " + "1";
        }

        public override bool AltFunctionUse(Player player)
        {
            Spellwright.Instance.spellInputState.Activate();
            //Spellwright.instance.userInterface.IsVisible = false;
            //Spellwright.instance.userInterface.Use();
            return false;
        }

        public override bool ConsumeItem(Player player)
        {
            return false;
        }

        //public override void NetSend(BinaryWriter writer)
        //{
        //    int size = SpellIds.Count;
        //    writer.Write(size);
        //    for (int i = 0; i < size; i++)
        //        writer.Write(SpellIds[i]);
        //}

        //public override void NetRecieve(BinaryReader reader)
        //{
        //    int size = reader.ReadInt32();
        //    SpellIds = new List<int>(size);
        //    for (int i = 0; i < size; i++)
        //        SpellIds.Add(reader.ReadInt32());
        //}
    }
}