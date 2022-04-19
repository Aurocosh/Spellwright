using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Modifiers;
using Spellwright.Network;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.ID;

namespace Spellwright.Content.Spells.WorldEvents
{
    internal class DesertRiteSpell : ModSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 5;
            UseType = SpellType.Invocation;

            AddApplicableModifier(SpellModifier.IsDispel);
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            if (!spellData.HasModifier(SpellModifier.IsDispel))
            {
                Sandstorm.StartSandstorm();
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    ModNetHandler.StartSandstormHandler.Send(true);
            }
            else
            {
                Sandstorm.StopSandstorm();
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    ModNetHandler.StopSandstormHandler.Send(true);
            }

            return true;
        }
    }
}