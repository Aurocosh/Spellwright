using Spellwright.Common.Players;
using Spellwright.Content.Spells.Base;
using Spellwright.DustSpawners;
using Spellwright.Extensions;
using Spellwright.Manipulators;
using Spellwright.Network;
using Terraria;
using Terraria.ID;

namespace Spellwright.Content.Spells.WorldEvents
{
    internal class AdventOfSummerSpell : ModSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 6;
            UseType = SpellType.Invocation;
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            var spellPlayer = player.GetModPlayer<SpellwrightPlayer>();

            int radius = 3 * spellPlayer.PlayerLevel;
            var centerPoint = player.Center.ToGridPoint();

            var herbGrower = new AreaHerbAndTreeGrower(centerPoint, radius);
            if (Main.netMode != NetmodeID.MultiplayerClient)
                herbGrower.Execute();
            else
                ModNetHandler.AreaHerbAndTreeGrowerHandler.Send(herbGrower);

            var spawner = new HerbAoeDustSpawner(player, radius);
            spawner.Execute();
            if (Main.netMode == NetmodeID.Server)
                ModNetHandler.HerbAoeDustHandler.Send(spawner);

            return true;
        }
    }
}