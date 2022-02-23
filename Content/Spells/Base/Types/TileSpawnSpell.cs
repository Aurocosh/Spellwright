using Microsoft.Xna.Framework;
using Spellwright.Extensions;
using Spellwright.Lib.Constants;
using Spellwright.Util;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace Spellwright.Content.Spells.Base.Types
{
    internal abstract class TileSpawnSpell : ModSpell
    {
        protected int tileType;

        protected virtual int GetTileType(int playerLevel) => tileType;

        protected virtual TilePlaceAction CanPlaceTile(Tile tile, int i, int j, int playerLevel)
        {
            return UtilTiles.CanReplaceTile(tile);
        }

        protected virtual IEnumerable<Point> GetTilePositions(Point center, Player player, int playerLevel, SpellData spellData)
        {
            yield return center;
        }

        public TileSpawnSpell()
        {
            UseType = SpellType.Spell;

            tileType = TileID.Dirt;
            useTimeMultiplier = 1f;
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            var center = player.Center.ToGridPoint();
            return PlaceTiles(center, player, playerLevel, spellData);
        }
        public override bool Cast(Player player, int playerLevel, SpellData spellData, IProjectileSource source, Vector2 position, Vector2 direction)
        {
            var center = Main.MouseWorld.ToGridPoint();
            return PlaceTiles(center, player, playerLevel, spellData);
        }

        private bool PlaceTiles(Point center, Player player, int playerLevel, SpellData spellData)
        {
            bool placedAtLeastOne = false;
            int tileType = GetTileType(playerLevel);
            var tilePositions = GetTilePositions(center, player, playerLevel, spellData);
            foreach (var point in tilePositions)
            {
                if (!WorldGen.InWorld(point.X, point.Y))
                    continue;
                Tile tile = Framing.GetTileSafely(point.X, point.Y);
                TilePlaceAction tilePlaceAction = CanPlaceTile(tile, point.X, point.Y, playerLevel);

                if (tilePlaceAction == TilePlaceAction.CanReplace)
                {
                    WorldGen.KillTile(point.X, point.Y, false, false, true);
                    var tileState = Framing.GetTileSafely(point.X, point.Y);
                    if (!tileState.HasTile && Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 4, point.X, point.Y);
                }
                if (tilePlaceAction == TilePlaceAction.CanPlace || tilePlaceAction == TilePlaceAction.CanReplace)
                {
                    bool placed = WorldGen.PlaceTile(point.X, point.Y, tileType, false, false, Main.myPlayer);
                    if (placed && Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        placedAtLeastOne = true;
                        NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 1, point.X, point.Y, tileType, 0);
                    }
                }
                else if (tile.Slope != 0)
                {
                    bool sloped = WorldGen.SlopeTile(point.X, point.Y);
                    if (sloped && Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 14, point.X, point.Y);
                }
            }

            return placedAtLeastOne;
        }
    }
}
