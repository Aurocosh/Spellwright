using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Spellwright.UI.Components
{
    internal class UIPanel : UIBase
    {
        public UIPanel()
        {
            width = 600f;
            height = 400f;
            BackgroundColor = new Color(33, 15, 91, 255) * 0.685f;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Visible)
                Utils.DrawInvBG(spriteBatch, AbsolutePosition.X, AbsolutePosition.Y, Width, Height, BackgroundColor);
            base.Draw(spriteBatch);
        }
    }
}