using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace Spellwright.Spells.Base
{
    internal abstract class TileSpawnSpell : Spell
    {
        protected int tileType;

        protected virtual int GetTileType(int playerLevel) => tileType;
        protected virtual bool CanReplaceTile(Tile tile, int i, int j, int playerLevel) => !tile.HasTile;

        public TileSpawnSpell(string name, string incantation, SpellType spellType = SpellType.Spell) : base(name, incantation, spellType)
        {
            tileType = ProjectileID.WoodenArrowFriendly;
            useTimeMultiplier = 1f;
        }
        public override bool Cast(Player player, int playerLevel, SpellData spellData, IProjectileSource source, Vector2 position, Vector2 direction)
        {
            direction.Normalize();

            Vector2 mousePosition = Main.MouseWorld;

            int xPos = (int)(mousePosition.X / 16.0f);
            int yPos = (int)(mousePosition.Y / 16.0f);

            if (!WorldGen.InWorld(xPos, yPos))
                return false;


            int tileType = GetTileType(playerLevel);
            Tile tile = Framing.GetTileSafely(xPos, yPos);

            bool canPlaceTile = CanReplaceTile(tile, xPos, yPos, playerLevel);
            if (!canPlaceTile)
                return false;

            bool placed = WorldGen.PlaceTile(xPos, yPos, tileType, false, false, Main.myPlayer);
            if (placed && Main.netMode == NetmodeID.MultiplayerClient)
                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 1, xPos, yPos, tileType, 0);

            return true;
        }
    }
}
