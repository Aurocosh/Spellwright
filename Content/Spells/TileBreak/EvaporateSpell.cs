using Microsoft.Xna.Framework;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.SpellCosts.Reagent;
using Spellwright.Content.Spells.Base.Types;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.TileBreak
{
    internal class EvaporateSpell : LiquidDestroySpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 3;
            UseType = SpellType.Invocation;
            liquidType = LiquidID.Water;
            var sound = SoundID.Item21;
            sound.PitchVariance = .3f;
            sound.Volume = .5f;
            castSound = sound;

            CastCost = new ReagentSpellCost(ModContent.ItemType<CommonSpellReagent>(), 10);
        }

        protected override void DoAreaEffect(Point point, Player player)
        {
            if (Main.rand.NextFloat() < .15f)
            {
                var direction = Main.rand.NextVector2Unit() * 2f;
                var position = point.ToWorldCoordinates(0, 0);
                var dust = Dust.NewDustDirect(position, 16, 16, DustID.Cloud, direction.X, direction.Y, 50, Color.White, .6f);
                dust.noGravity = true;
            }
        }
    }
}