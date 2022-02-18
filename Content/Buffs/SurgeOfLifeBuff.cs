using Microsoft.Xna.Framework;
using Spellwright.Util;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Spellwright.Content.Buffs
{
    public class SurgeOfLifeBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Surge of life");
            Description.SetDefault("Boosts regeneration of health");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<SurgeOfLifePlayer>().LifeRegenBuffed = true;
        }
    }

    public class SurgeOfLifePlayer : ModPlayer
    {
        // Flag checking when life regen debuff should be activated
        public bool LifeRegenBuffed { get; set; } = false;
        public int LifeRegenValue { get; set; } = 2;

        public override void ResetEffects()
        {
            LifeRegenBuffed = false;
        }

        public override void UpdateLifeRegen()
        {
            if (!LifeRegenBuffed)
                return;

            int x = (int)Player.Center.X / 16;
            int y = (int)(Player.position.Y + Player.height - 1f) / 16;

            bool isStandingOnGrass = false;
            Tile tileAtFeet = Main.tile[x, y];
            Tile tileUnderFeet = Main.tile[x, y + 1];
            if (tileAtFeet != null && tileUnderFeet != null)
            {
                if (tileAtFeet.LiquidAmount == 0 && WorldGen.SolidTile(x, y + 1))
                {
                    int tileType = tileUnderFeet.TileType;
                    isStandingOnGrass = UtilTiles.IsTileGrass(tileType);
                }
            }

            int regenValue = LifeRegenValue;

            if (isStandingOnGrass)
            {
                regenValue *= 2;

                Vector2 position = Player.Center;
                position.X -= 3;

                Vector2 dustPosition = UtilVector2.GetPointOnEllipse(position, 25, 50);
                var dust = Dust.NewDustDirect(dustPosition, 0, 0, DustID.GreenTorch, 0f, 0f, 100, default, 1.5f);
                dust.noLightEmittence = true;
                dust.noGravity = true;
            }

            Player.lifeRegenTime += (int)(Player.lifeRegenTime * 0.2f);
            Player.lifeRegen += regenValue;
        }
        public override void SaveData(TagCompound tag)
        {
            tag.Add("LifeRegenBuffed", LifeRegenBuffed);
            tag.Add("LifeRegenValue", LifeRegenValue);
        }

        public override void LoadData(TagCompound tag)
        {
            LifeRegenBuffed = tag.GetBool("LifeRegenBuffed");
            LifeRegenValue = tag.GetInt("LifeRegenValue");
        }
    }
}
