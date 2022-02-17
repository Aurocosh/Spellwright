using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace Spellwright.UI
{
    internal class UiSystem : ModSystem
    {
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient && !Spellwright.IsPlayerServerOwner(Main.LocalPlayer))
                return;

            int MouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (MouseTextIndex != -1)
                layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
                    "Spellwright: Spell input",
                    delegate
                    {
                        DrawUpdateAll(Main.spriteBatch);
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
        }
        private static void DrawUpdateAll(SpriteBatch spriteBatch)
        {
            Spellwright.instance.spellInput.Draw(spriteBatch);
            Spellwright.instance.spellInput.Update();

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Main.UIScaleMatrix);
        }
    }
}