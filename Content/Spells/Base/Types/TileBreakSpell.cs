using Microsoft.Xna.Framework;
using Spellwright.Extensions;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace Spellwright.Content.Spells.Base.Types
{
    internal abstract class TileBreakSpell : ModSpell
    {
        protected int tileType;
        protected bool noItem;

        protected virtual bool CanBreakTile(Tile tile, int x, int y, int playerLevel)
        {
            return tile != null && tile.TileType == tileType;
        }

        protected virtual IEnumerable<Point> GetTilePositions(Point center, Player player, int playerLevel, SpellData spellData)
        {
            yield return center;
        }

        public TileBreakSpell()
        {
            UseType = SpellType.Spell;

            tileType = TileID.Dirt;
            noItem = true;
            useTimeMultiplier = 1f;
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            var center = player.Center.ToGridPoint();
            return BreakTiles(center, player, playerLevel, spellData);
        }
        public override bool Cast(Player player, int playerLevel, SpellData spellData, IProjectileSource source, Vector2 position, Vector2 direction)
        {
            var center = Main.MouseWorld.ToGridPoint();
            return BreakTiles(center, player, playerLevel, spellData);
        }

        private bool BreakTiles(Point center, Player player, int playerLevel, SpellData spellData)
        {
            bool brokeAtLeastOne = false;
            var tilePositions = GetTilePositions(center, player, playerLevel, spellData);
            foreach (var point in tilePositions)
            {
                if (!WorldGen.InWorld(point.X, point.Y))
                    continue;
                Tile tile = Framing.GetTileSafely(point.X, point.Y);
                if (!CanBreakTile(tile, point.X, point.Y, playerLevel))
                    continue;

                WorldGen.KillTile(point.X, point.Y, false, false, noItem);
                var tileState = Framing.GetTileSafely(point.X, point.Y);
                if (!tileState.HasTile && Main.netMode == NetmodeID.MultiplayerClient)
                {
                    brokeAtLeastOne = true;
                    NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, point.X, point.Y);
                }
            }

            return brokeAtLeastOne;
        }
    }
}
