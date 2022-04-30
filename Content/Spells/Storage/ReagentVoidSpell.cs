using Spellwright.Common.Players;
using Spellwright.Content.Items.Mirrors;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Content.Spells.Base.SpellCosts.Reagent;
using Spellwright.Content.Spells.Storage.Base;
using Spellwright.Extensions;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.Storage
{
    internal sealed class ReagentVoidSpell : StorageSpell
    {
        private readonly HashSet<int> acceptableItems = new();

        public override void SetStaticDefaults()
        {
            SpellLevel = 2;
            UseType = SpellType.Invocation;

            UnlockCost = new MultipleItemSpellCost()
                .WithCost(ModContent.ItemType<RareSpellReagent>(), 5)
                .WithCost(ItemID.RecallPotion, 5);

            CastCost = new ReagentSpellCost(ModContent.ItemType<CommonSpellReagent>(), 10);

            acceptableItems.Clear();
            acceptableItems.Add(ModContent.ItemType<CommonSpellReagent>());
            acceptableItems.Add(ModContent.ItemType<RareSpellReagent>());
            acceptableItems.Add(ModContent.ItemType<MythicalSpellReagent>());
            acceptableItems.Add(ModContent.ItemType<SilverMirror>());
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