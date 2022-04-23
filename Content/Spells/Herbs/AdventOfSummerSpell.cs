using Spellwright.Common.Players;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.ExecutablePackets.Broadcast.DustSpawners;
using Spellwright.ExecutablePackets.ToServer;
using Spellwright.Extensions;
using Spellwright.Network;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.Herbs
{
    internal class AdventOfSummerSpell : ModSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 9;
            UseType = SpellType.Invocation;

            UnlockCost = new MultipleItemSpellCost()
                .WithCost(ItemID.Sunflower, 1)
                .WithCost(ItemID.ChlorophyteBar, 30);
            SpellCost = new ReagentSpellCost(ModContent.ItemType<MythicalSpellReagent>(), 1);
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