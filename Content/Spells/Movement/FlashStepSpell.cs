using Microsoft.Xna.Framework;
using Spellwright.Common.Players;
using Spellwright.Content.Spells.Base;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;

namespace Spellwright.Content.Spells.Movement
{
    internal class FlashStepSpell : ModSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 2;
            UseType = SpellType.Cantrip;
            useDelay = 80;
            useSound = new LegacySoundStyle(SoundID.Run, 0);
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData, IEntitySource source, Vector2 position, Vector2 direction)
        {
            SpellwrightDashPlayer dashPlayer = player.GetModPlayer<SpellwrightDashPlayer>();
            if (!dashPlayer.CanUseDash())
                return false;

            var newVelocity = new Vector2(player.direction * 20, 1);
            dashPlayer.Dash(newVelocity, 60);

            return true;
        }
    }
}