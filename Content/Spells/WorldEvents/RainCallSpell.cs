using Spellwright.Content.Spells.Base;
using Spellwright.Network;
using Terraria;
using Terraria.ID;

namespace Spellwright.Content.Spells.WorldEvents
{
    internal class RainCallSpell : ModSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 4;
            UseType = SpellType.Invocation;
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            Main.StartRain();
            if (Main.netMode == NetmodeID.MultiplayerClient)
                ModNetHandler.startRainHandler.Send(true);
            return true;
        }
    }
}