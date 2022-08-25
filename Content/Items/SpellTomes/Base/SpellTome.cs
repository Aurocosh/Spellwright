using Microsoft.Xna.Framework;
using Spellwright.Common.Players;
using Spellwright.Content.Spells.Base;
using Spellwright.ExecutablePackets.Broadcast.DustSpawners;
using Spellwright.MyLibs.Randoms;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Spellwright.Content.Items.SpellTomes.Base
{
    public abstract class SpellTome : ModItem
    {
        protected static readonly Dictionary<int, SpellTomeContent> tomeContents = new();

        protected static void Add(int type, SpellTomeContent tomeContent)
        {
            tomeContents.Add(type, tomeContent);
        }

        public override void SetDefaults()
        {
            Item.consumable = true;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 30;
            Item.maxStack = 30;
            Item.UseSound = SoundID.Item4;
            Item.useAnimation = 30;
            Item.value = Item.buyPrice(0, 0, 10);
            Item.rare = ItemRarityID.Blue;
        }

        public SpellTomeContent GetContent()
        {
            if (tomeContents.TryGetValue(Type, out var tome))
                return tome;
            return null;
        }

        public override bool? UseItem(Player player)
        {
            SpellwrightPlayer spellPlayer = player.GetModPlayer<SpellwrightPlayer>();
            if (!spellPlayer.CanCastSpells)
                return false;
            if (!tomeContents.TryGetValue(Type, out var tome))
                return false;

            var unknownSpells = new List<ModSpell>();
            foreach (var spell in tome.Spells)
                if (!spellPlayer.KnownSpells.Contains(spell.Type))
                    unknownSpells.Add(spell);

            if (unknownSpells.Count == 0)
                return false;

            var possibleSpells = new DistributedRandom<ModSpell>();
            foreach (var modSpell in unknownSpells)
            {
                var distribution = tome.SpellDistributions[modSpell];
                possibleSpells.Add(modSpell, distribution);
            }

            int spellsToAdd = tome.SpellCounts.GetRandomItem(Main.rand.NextDouble());
            spellsToAdd = Math.Min(spellsToAdd, unknownSpells.Count);

            var learnedSpells = new List<ModSpell>();
            for (int i = 0; i < spellsToAdd; i++)
            {
                var spell = possibleSpells.GetRandomItem(Main.rand.NextDouble());
                spellPlayer.KnownSpells.Add(spell.Type);
                possibleSpells.Remove(spell);
                learnedSpells.Add(spell);
            }

            var learnedNames = learnedSpells.Select(x => x.DisplayName.GetTranslation(Language.ActiveCulture));
            var names = string.Join(", ", learnedNames);
            var message = Spellwright.GetTranslation("General", "SpellsLearned");
            Main.NewText(message.Format(names), Color.White);

            var spawner = new LevelUpDustSpawner(player, learnedSpells.Select(x => x.SpellLevel));
            spawner.Execute();

            return true;
        }
    }
}