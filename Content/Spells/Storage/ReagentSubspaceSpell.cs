using Spellwright.Common.Players;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Extensions;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.Storage
{
    internal sealed class ReagentSubspaceSpell : StorageSpell
    {
        private readonly HashSet<int> acceptableItems = new();

        public override void SetStaticDefaults()
        {
            SpellLevel = 6;
            UseType = SpellType.Invocation;

            UnlockCost = new MultipleItemSpellCost()
                .WithCost(ItemID.SoulofNight, 5)
                .WithCost(ItemID.TeleportationPotion, 5);

            SpellCost = new ReagentSpellCost(ModContent.ItemType<RareSpellReagent>(), 1);

            acceptableItems.Clear();
            acceptableItems.Add(ModContent.ItemType<CommonSpellReagent>());
            acceptableItems.Add(ModContent.ItemType<RareSpellReagent>());
            acceptableItems.Add(ModContent.ItemType<MythicalSpellReagent>());
        }

        protected override bool CanAccept(Item item)
        {
            return acceptableItems.Contains(item.type);
        }

        protected override List<Item> GetStorage(Player player)
        {
            var statPlayer = player.GetModPlayer<SpellwrightStatPlayer>();
            return statPlayer.ReagentItems;
        }

        protected override int StorageSize(int playerLevel)
        {
            return playerLevel;
        }

        protected override InventoryArea IncludedArea()
        {
            return InventoryArea.All;
        }
    }
}