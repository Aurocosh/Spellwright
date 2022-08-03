using Spellwright.Common.Players;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Content.Spells.Base.SpellCosts.Reagent;
using Spellwright.ExecutablePackets.Broadcast.DustSpawners;
using Spellwright.Extensions;
using Spellwright.NetworkActions.ServerActions;
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
            CastCost = new ReagentSpellCost(ModContent.ItemType<MythicalSpellReagent>(), 1);
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            var spellPlayer = player.GetModPlayer<SpellwrightPlayer>();

            int radius = 4 * spellPlayer.PlayerLevel;
            var centerPoint = player.Center.ToGridPoint();

            var herbGrower = new AreaHerbAndTreeGrower(centerPoint, radius);
            herbGrower.Execute();

            var spawner = new HerbAoeDustSpawner(player, radius);
            spawner.Execute();

            return true;
        }
    }
}