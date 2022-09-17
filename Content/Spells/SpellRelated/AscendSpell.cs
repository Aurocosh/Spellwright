using Spellwright.Common.Players;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.SpellCosts;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.ExecutablePackets.Broadcast.DustSpawners;
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
            castSound = SoundID.Item4;
            costIsImportant = true;

            mappedSpellCost.ClearCosts();
            mappedSpellCost.SetSpellCost(0, new SingleItemSpellCost(ItemID.FallenStar, 30));
            mappedSpellCost.SetSpellCost(1, new OptionalSpellCost().WithCost(ItemID.DemoniteBar, 50).WithCost(ItemID.CrimtaneBar, 50));
            mappedSpellCost.SetSpellCost(2, new SingleItemSpellCost(ItemID.MeteoriteBar, 40));
            mappedSpellCost.SetSpellCost(3, new SingleItemSpellCost(ItemID.HellstoneBar, 60));
            mappedSpellCost.SetSpellCost(4, new OptionalSpellCost().WithCost(ItemID.SoulofLight, 30).WithCost(ItemID.SoulofNight, 30));
            mappedSpellCost.SetSpellCost(5, new OptionalSpellCost().WithCost(ItemID.SoulofSight, 20).WithCost(ItemID.SoulofFright, 20).WithCost(ItemID.SoulofMight, 20));
            mappedSpellCost.SetSpellCost(6, new SingleItemSpellCost(ItemID.ChlorophyteBar, 60));
            mappedSpellCost.SetSpellCost(7, new SingleItemSpellCost(ItemID.Ectoplasm, 30));
            mappedSpellCost.SetSpellCost(8, new SingleItemSpellCost(ItemID.SpookyWood, 600));
            mappedSpellCost.SetSpellCost(9, new OptionalSpellCost().WithCost(ItemID.FragmentNebula, 30).WithCost(ItemID.FragmentSolar, 30).WithCost(ItemID.FragmentStardust, 30).WithCost(ItemID.FragmentVortex, 30));

            CastCost = mappedSpellCost;
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            var spellPlayer = player.GetModPlayer<SpellwrightPlayer>();
            spellPlayer.PlayerLevel += 1;

            var buffPlayer = player.GetModPlayer<SpellwrightBuffPlayer>();
            foreach (var buffId in buffPlayer.BuffLevels.Keys)
                buffPlayer.BuffLevels[buffId] = spellPlayer.PlayerLevel;

            return true;
        }
        public override void DoCastEffect(Player player, int playerLevel)
        {
            var spawner = new LevelUpDustSpawner(player, Enumerable.Range(1, playerLevel + 1));
            spawner.Execute();
        }
    }
}
