using Spellwright.Common.Players;
using Spellwright.Content.Spells.Base;
using Spellwright.ExecutablePackets.Broadcast.DustSpawners;
using Spellwright.Network;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
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
            Item.maxStack = 1;
            Item.UseSound = SoundID.Item4;
            Item.useAnimation = 30;
            Item.value = Item.buyPrice(0, 0, 10);
            Item.rare = ItemRarityID.Blue;
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

            foreach (var spell in unknownSpells)
                spellPlayer.KnownSpells.Add(spell.Type);

            var spawner = new LevelUpDustSpawner(player, unknownSpells.Select(x => x.SpellLevel));
            spawner.Execute();
            if (Main.netMode == NetmodeID.MultiplayerClient)
                ModNetHandler.levelUpDustHandler.Send(spawner);

            return true;
        }
    }
}