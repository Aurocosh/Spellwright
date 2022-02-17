using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.GameContent;

namespace Spellwright.UI.Components
{
    internal class UILabel : UIBase
    {
        private string text = "";
        public DynamicSpriteFont Font { get; set; }

        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
                RefreshDimensions();
            }
        }

        public UILabel()
        {
            Font = FontAssets.DeathText.Value;
            Text = "";
        }
        public UILabel(string text)
        {
            Font = FontAssets.DeathText.Value;
            Text = text;
        }

        private void RefreshDimensions()
        {
            if (Text != null)
            {
                Vector2 vector = Font.MeasureString(Text);
                width = vector.X;
                height = vector.Y;
            }
            else
            {
                width = 0f;
                height = 0f;
            }
        }

        public override float Width
        {
            get { return width * Scale; }
        }

        public override float Height
        {
            get
            {
                if (height == 0f)
                    return Font.MeasureString("Y").Y * Scale;
                return height * Scale;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Text != null)
                spriteBatch.DrawString(Font, Text, AbsolutePosition, ForegroundColor, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
            base.Draw(spriteBatch);
        }
    }
}