using Spellwright.Common.Players;
using Spellwright.Content.Items.SpellTomes.Base;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Healing;
using Spellwright.Content.Spells.Projectiles;
using Spellwright.Content.Spells.SpellRelated;
using Spellwright.Content.Spells.Warp;
using Spellwright.ExecutablePackets.Broadcast.DustSpawners;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;

namespace Spellwright.Content.Items.SpellTomes
{
    public class SpellwrightTeachings : SpellTome
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spellwright Teachings");
            Tooltip.SetDefault("Sacred book of Spellwrights that contains essential knowlege for those who wish to learn mystical art of spellcraft.");

            var content = new SpellTomeContent();

            // Level 0
            content.AddSpell<AscendSpell>();
            content.AddSpell<MendSpell>();
            content.AddSpell<ReturnSpell>();
            content.AddSpell<SparkCasterSpell>();

            Add(Type, content);
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.value = Item.buyPrice(0, 0, 10);
            Item.rare = ItemRarityID.Blue;
        }

        public override bool? UseItem(Player player)
        {
            SpellwrightPlayer spellPlayer = player.GetModPlayer<SpellwrightPlayer>();
            if (!tomeContents.TryGetValue(Type, out var tome))
                return false;

            var unknownSpells = new List<ModSpell>();
            foreach (var spell in tome.Spells)
                if (!spellPlayer.KnownSpells.Contains(spell.Type))
                    unknownSpells.Add(spell);

            if (spellPlayer.LearnedBasics && unknownSpells.Count == 0)
                return false;

            foreach (var spell in unknownSpells)
                spellPlayer.KnownSpells.Add(spell.Type);

            spellPlayer.LearnedBasics = true;

            var spawner = new LevelUpDustSpawner(player, unknownSpells.Select(x => x.SpellLevel));
            spawner.Execute();

            return true;
        }
    }
}