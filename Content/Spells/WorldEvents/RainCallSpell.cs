using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Modifiers;
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

            AddApplicableModifier(SpellModifier.IsDispel);
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            if (!spellData.HasModifier(SpellModifier.IsDispel))
            {
                Main.StartRain();
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    ModNetHandler.StartRainHandler.Send(true);
            }
            else
            {
                Main.StopRain();
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    ModNetHandler.StopRainHandler.Send(true);
            }

            return true;
        }
    }
}