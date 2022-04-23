using Spellwright.Common.Players;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.SpellCosts;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.ExecutablePackets.Broadcast.DustSpawners;
using Spellwright.Network;
using System.Linq;
using Terraria;
using Terraria.ID;

namespace Spellwright.Content.Spells.SpellRelated
{
    internal class AscendSpell : ModSpell
    {
        private readonly LevelMappedSpellCost mappedSpellCost;

        public AscendSpell()
        {
            mappedSpellCost = new LevelMappedSpellCost(Spellwright.GetTranslation("Messages", "PlayerLevelIsAlreadyMaxed").Value);
        }

        public SpellCost GetLevelUpCost(int level) => mappedSpellCost.GetSpellCost(level);

        public override void SetStaticDefaults()
        {
            SpellLevel = 0;
            UseType = SpellType.Invocation;

            mappedSpellCost.ClearCosts();
            mappedSpellCost.SetSpellCost(0, new SingleItemSpellCost(ItemID.FallenStar, 30));
            mappedSpellCost.SetSpellCost(1, new SingleItemSpellCost(ItemID.MeteoriteBar, 40));
            mappedSpellCost.SetSpellCost(2, new SingleItemSpellCost(ItemID.DemoniteBar, 50));
            mappedSpellCost.SetSpellCost(3, new SingleItemSpellCost(ItemID.HellstoneBar, 60));
            mappedSpellCost.SetSpellCost(4, new OptionalSpellCost(new SingleItemSpellCost(ItemID.SoulofLight, 30), new SingleItemSpellCost(ItemID.SoulofNight, 30)));
            mappedSpellCost.SetSpellCost(5, new SingleItemSpellCost(ItemID.ChlorophyteBar, 60));
            mappedSpellCost.SetSpellCost(6, new OptionalSpellCost(new SingleItemSpellCost(ItemID.SoulofSight, 30), new SingleItemSpellCost(ItemID.SoulofFright, 30), new SingleItemSpellCost(ItemID.SoulofMight, 30)));
            mappedSpellCost.SetSpellCost(7, new SingleItemSpellCost(ItemID.Ectoplasm, 30));
            mappedSpellCost.SetSpellCost(8, new SingleItemSpellCost(ItemID.SpookyWood, 600));
            mappedSpellCost.SetSpellCost(9, new OptionalSpellCost(new SingleItemSpellCost(ItemID.FragmentNebula, 30), new SingleItemSpellCost(ItemID.FragmentSolar, 30), new SingleItemSpellCost(ItemID.FragmentStardust, 30), new SingleItemSpellCost(ItemID.FragmentVortex, 30)));

            SpellCost = mappedSpellCost;
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            if (spellData == null)
                return false;

            var spellPlayer = player.GetModPlayer<SpellwrightPlayer>();
            spellPlayer.PlayerLevel += 1;

            var buffPlayer = player.GetModPlayer<SpellwrightBuffPlayer>();
            foreach (var buffId in buffPlayer.BuffLevels.Keys)
                buffPlayer.BuffLevels[buffId] = spellPlayer.PlayerLevel;

            var spawner = new LevelUpDustSpawner(player, Enumerable.Range(1, spellPlayer.PlayerLevel));
            spawner.Execute();
            if (Main.netMode == NetmodeID.MultiplayerClient)
                ModNetHandler.levelUpDustHandler.Send(spawner);

            return true;
        }
    }
}
