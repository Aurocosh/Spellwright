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
using Terraria.ModLoader.IO;

namespace Spellwright.Content.Items
{
    public class SpellResonator : ModItem
    {
        public ModSpell CurrentSpell = null;
        public SpellData SpellData = null;
        public int SpellUsesLeft = 0;

        public SpellResonator()
        {
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spell Resonator");
            Tooltip.SetDefault("Magical artefact resonating with your voice and capable of binding your words to itself.");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.useTime = 100;
            Item.useAnimation = 100;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = false;
            Item.damage = 0;
            Item.DamageType = DamageClass.Magic;
            Item.width = 32;
            Item.height = 32;
            Item.shoot = ProjectileID.FireArrow;
            Item.useAmmo = AmmoID.None;
            Item.noMelee = true; // Makes the item not do damage with it's melee hitbox.
            Item.shootSpeed = 7; // How fast the item shoots the projectile.

            Item.value = Item.buyPrice(0, 0, 50);
            Item.rare = ItemRarityID.Red;
        }

        public void UpdateName()
        {
            if (CurrentSpell == null)
            {
                Item.ClearNameOverride();
            }
            else
            {
                var itemName = Lang.GetItemNameValue(Type);
                var spellName = CurrentSpell.DisplayName.GetTranslation(Language.ActiveCulture);
                if (SpellUsesLeft > 0)
                    Item.SetNameOverride($"{itemName} ({spellName} - {SpellUsesLeft})");
                else
                    Item.SetNameOverride($"{itemName} ({spellName})");
            }
        }

        public override ModItem Clone(Item item)
        {
            var clone = (SpellResonator)base.Clone(item);
            //clone.SpellIds = new List<int>(SpellIds);
            return clone;
        }

        public override float UseSpeedMultiplier(Player player)
        {
            SpellwrightPlayer spellwrightPlayer = Main.LocalPlayer.GetModPlayer<SpellwrightPlayer>();
            int playerLevel = spellwrightPlayer.PlayerLevel;
            return CurrentSpell?.GetUseSpeedMultiplier(playerLevel) ?? 4f;
        }

        public override bool? CanAutoReuseItem(Player player)
        {
            SpellwrightPlayer spellwrightPlayer = Main.LocalPlayer.GetModPlayer<SpellwrightPlayer>();
            int playerLevel = spellwrightPlayer.PlayerLevel;
            return CurrentSpell?.CanAutoReuse(playerLevel) ?? false;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var spellPlayer = SpellwrightPlayer.Instance;
            if (!spellPlayer.CanCastSpells)
                return false;

            int playerLevel = spellPlayer.PlayerLevel;
            if (CurrentSpell != null && SpellData != null)
            {
                if (!CurrentSpell.ConsumeReagentsUse(player, playerLevel, SpellData))
                    return false;

                bool canCast = false;
                bool consumeCharge = false;
                if (SpellUsesLeft > 0)
                {
                    canCast = true;
                    consumeCharge = true;
                }
                else
                {
                    //float stability = CurrentSpell.GetStability(playerLevel);
                    //if (stability > 0)
                    //{
                    //    var randomRoll = Main.rand.NextDouble();
                    //    if (randomRoll < stability)
                    //        canCast = true;
                    //}
                }

                if (canCast)
                {
                    bool success = CurrentSpell.Cast(player, playerLevel, SpellData, source, position, velocity);
                    if (success && consumeCharge)
                    {
                        SpellUsesLeft--;
                        UpdateName();
                    }
                }
                else
                {
                    CurrentSpell = null;
                    SpellData = null;
                    UpdateName();
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
            TooltipLine itemName = tooltips[0];
            TooltipLine description = null;
            TooltipLine price = null;

            foreach (var line in tooltips)
            {
                if (line.Name == "Tooltip0")
                    description = line;
                else if (line.Name == "Price")
                    price = line;
            }

            tooltips.Clear();
            tooltips.Add(itemName);

            Spellwright spellwright = Spellwright.Instance;
            Player player = Main.LocalPlayer;
            SpellwrightPlayer spellwrightPlayer = player.GetModPlayer<SpellwrightPlayer>();
            int playerLevel = spellwrightPlayer.PlayerLevel;
            if (CurrentSpell == null)
            {
                if (description != null)
                    tooltips.Add(description);
                tooltips.Add(new TooltipLine(spellwright, "", "You have no active spells"));
            }
            else
            {
                string name = CurrentSpell.DisplayName.GetTranslation(Language.ActiveCulture);
                tooltips.Add(new TooltipLine(spellwright, "Spell name", name));

                var descriptionValues = CurrentSpell.GetDescriptionValues(player, playerLevel, SpellData, false);
                foreach (var value in descriptionValues)
                {
                    var parameterName = Spellwright.GetTranslation("DescriptionParts", value.Name).Value;
                    var descriptionPart = $"{parameterName}: {value.Value}";
                    tooltips.Add(new TooltipLine(spellwright, parameterName, descriptionPart));
                }

                if (SpellUsesLeft > 0)
                {
                    var parameterName = Spellwright.GetTranslation("DescriptionParts", "SpellUses").Value;
                    var descriptionPart = $"{parameterName}: {SpellUsesLeft}";
                    tooltips.Add(new TooltipLine(spellwright, parameterName, descriptionPart));
                }

                //string description = CurrentSpell.Description.GetTranslation(Language.ActiveCulture);
                //tooltips.Add(new TooltipLine(spellwright, "Description", $"Description: {description}"));
            }

            if (price != null)
                tooltips.Add(price);
        }

        public override bool AltFunctionUse(Player player)
        {
            if (SpellwrightPlayer.Instance.CanCastSpells)
                Spellwright.Instance.userInterface.SetState(Spellwright.Instance.spellInputState);
            return false;
        }

        public override bool ConsumeItem(Player player)
        {
            return false;
        }

        public override void SaveData(TagCompound tag)
        {
            tag.Add("SpellUsesLeft", SpellUsesLeft);

            if (CurrentSpell != null && SpellData != null)
            {
                tag.Add("CurrentSpell", CurrentSpell.Name ?? "");
                tag.Add("CurrentSpellData", CurrentSpell.SerializeData(SpellData));
            }
        }

        public override void LoadData(TagCompound tag)
        {
            SpellUsesLeft = tag.GetInt("SpellUsesLeft");

            string spellName = tag.GetString("CurrentSpell");
            if (ModContent.TryFind(Spellwright.Instance.Name, spellName, out CurrentSpell))
            {
                TagCompound spellDataTag = tag.GetCompound("CurrentSpellData");
                SpellData = CurrentSpell.DeserializeData(spellDataTag);
            }
            UpdateName();
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