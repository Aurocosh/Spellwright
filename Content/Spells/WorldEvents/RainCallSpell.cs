using Spellwright.Content.Spells.Base;
using Spellwright.Network;
using Terraria;
using Terraria.ID;

namespace Spellwright.Content.Spells.Enchant
{
    internal class RainCallSpell : ModSpell
    {
        public override void SetStaticDefaults()
        {
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