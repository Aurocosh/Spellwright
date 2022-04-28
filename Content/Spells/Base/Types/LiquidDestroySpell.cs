using Microsoft.Xna.Framework;
using Spellwright.Extensions;
using Spellwright.Lib.Constants;
using Spellwright.Lib.Primitives;
using Spellwright.Util;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace Spellwright.Content.Spells.Base.Types
{
    internal abstract class LiquidDestroySpell : ModSpell
    {
        protected int liquidType;

        protected virtual int GetLiquidType(int playerLevel) => liquidType;
        protected virtual void DoAreaEffect(Point point, Player player) { }


        protected virtual IEnumerable<Point> GetTilePositions(Point center, Player player, int playerLevel, SpellData spellData)
        {
            var circle = new SolidCircle(center, 20);

            bool IsValid(Point point)
            {
                if (!circle.IsInBounds(point) || !WorldGen.InWorld(point.X, point.Y))
                    return false;
                Tile tile = Framing.GetTileSafely(point.X, point.Y);
                if (WorldGen.SolidTile(tile))
                    return false;
                return true;
            }

            var coords = new SolidCircle(center, 3);
            return UtilCoordinates.FloodFill(coords, PointConstants.DirectNeighbours, IsValid, 1000);
        }

        public LiquidDestroySpell()
        {
            UseType = SpellType.Invocation;
            liquidType = LiquidID.Water;
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            int currentLiquidType = GetLiquidType(playerLevel);

            var center = player.Center.ToGridPoint();

            bool deletedAtLeastOne = false;
            var tilePositions = GetTilePositions(center, player, playerLevel, spellData);
            foreach (var point in tilePositions)
            {
                DoAreaEffect(point, player);

                Tile tile = Framing.GetTileSafely(point.X, point.Y);
                int tileLiquidType = tile.LiquidType;
                if (tile.LiquidAmount > 0 && (liquidType == -1 || tileLiquidType == currentLiquidType))
                {
                    tile.LiquidType = LiquidID.Lava;
                    tile.LiquidAmount = 0;
                    WorldGen.SquareTileFrame(point.X, point.Y, false);
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.sendWater(point.X, point.Y);
                    else
                        Liquid.AddWater(point.X, point.Y);
                    deletedAtLeastOne = true;
                }
            }

            return deletedAtLeastOne;
        }
    }
}
