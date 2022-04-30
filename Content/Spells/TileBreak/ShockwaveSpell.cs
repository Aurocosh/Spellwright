using Microsoft.Xna.Framework;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.SpellCosts.Reagent;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Lib.Constants;
using Spellwright.Lib.PointShapes;
using Spellwright.Util;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.TileBreak
{
    internal class ShockwaveSpell : TileBreakSpell
    {
        private static HashSet<int> breakableTileTypes = new();

        public override void SetStaticDefaults()
        {
            SpellLevel = 2;
            UseType = SpellType.Invocation;
            noItem = false;
            castSound = SoundID.Item14.WithPitchVariance(.3f).WithVolume(.5f);

            CastCost = new ReagentSpellCost(ModContent.ItemType<CommonSpellReagent>(), 10);

            breakableTileTypes.Clear();
            breakableTileTypes.Add(TileID.BreakableIce);
            breakableTileTypes.Add(TileID.Plants);
            breakableTileTypes.Add(TileID.Plants2);
            breakableTileTypes.Add(TileID.CorruptPlants);
            breakableTileTypes.Add(TileID.CrimsonPlants);
            breakableTileTypes.Add(TileID.HallowedPlants);
            breakableTileTypes.Add(TileID.HallowedPlants2);
            breakableTileTypes.Add(TileID.JunglePlants);
            breakableTileTypes.Add(TileID.JunglePlants2);
            breakableTileTypes.Add(TileID.MushroomPlants);
            breakableTileTypes.Add(TileID.OasisPlants);
            breakableTileTypes.Add(TileID.Vines);
            breakableTileTypes.Add(TileID.CrimsonVines);
            breakableTileTypes.Add(TileID.HallowedVines);
            breakableTileTypes.Add(TileID.JungleVines);
            breakableTileTypes.Add(TileID.MushroomVines);
            breakableTileTypes.Add(TileID.JungleThorns);
            breakableTileTypes.Add(TileID.CorruptThorns);
            breakableTileTypes.Add(TileID.CrimsonThorns);
            breakableTileTypes.Add(TileID.Cobweb);
        }

        protected override bool CanBreakTile(Tile tile, int x, int y, int playerLevel)
        {
            return breakableTileTypes.Contains(tile.TileType);
        }

        protected override void DoAreaEffect(Point point, Player player)
        {
            if (Main.rand.NextFloat() < .15f)
            {
                var direction = (point.ToWorldCoordinates().DirectionFrom(player.Center)) * 10;
                var position = point.ToWorldCoordinates(0, 0);
                var dust = Dust.NewDustDirect(position, 16, 16, DustID.CorruptPlants, direction.X, direction.Y, 50, Color.White, 1.2f);
                dust.noGravity = true;
            }
        }

        protected override IEnumerable<Point> GetTilePositions(Point center, Player player, int playerLevel, SpellData spellData)
        {
            var circle = new SolidCircle(center, 40);
            Vector2 playerCenter = player.Center;

            bool IsValid(Point point)
            {
                if (!circle.IsInBounds(point) || !WorldGen.InWorld(point.X, point.Y))
                    return false;
                Tile tile = Main.tile[point.X, point.Y];
                if (WorldGen.SolidTile(tile))
                    return false;
                return true;
            }

            return UtilCoordinates.FloodFill(new[] { center }, PointConstants.DirectNeighbours, IsValid, 3000);
        }
    }
}