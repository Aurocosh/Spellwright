using Microsoft.Xna.Framework;
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
                        SpriteBatch spriteBatch = Main.spriteBatch;
                        Spellwright.Instance.userInterface?.Draw(spriteBatch, new GameTime());

                        spriteBatch.End();
                        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Main.UIScaleMatrix);
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
        }

        public override void UpdateUI(GameTime gameTime)
        {
            Spellwright.Instance.userInterface?.Update(gameTime);
        }
    }
}