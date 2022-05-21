using Microsoft.Xna.Framework;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.SpellCosts.Reagent;
using Spellwright.Core.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.TileBreak
{
    internal class ShockwaveSpell : ModSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 2;
            UseType = SpellType.Invocation;
            castSound = SoundID.Item14.WithPitchVariance(.3f).WithVolume(.8f);
            CastCost = new ReagentSpellCost(ModContent.ItemType<CommonSpellReagent>(), 10);
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            var shockwaveHandler = new ShockwaveHandler();
            Vector2 centerPosition = player.Center;
            return shockwaveHandler.DoShockwave(centerPosition, 40);
        }
    }
}