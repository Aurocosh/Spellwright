using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;

namespace Spellwright.UI.Components
{
    internal class UIPanel : UIBase
    {
        public UIPanel()
        {
            Width = new StyleDimension(600, 0);
            Height = new StyleDimension(400, 0);

            BackgroundColor = new Color(33, 15, 91, 255) * 0.685f;
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = GetDimensions();
            var point = new Point((int)dimensions.X, (int)dimensions.Y);
            Utils.DrawInvBG(spriteBatch, point.X, point.Y, dimensions.Width, dimensions.Height, BackgroundColor);
        }
    }
}