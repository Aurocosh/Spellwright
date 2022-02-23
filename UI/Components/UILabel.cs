using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.GameContent;
using Terraria.UI;

namespace Spellwright.UI.Components
{
    internal class UILabel : UIBase
    {
        private string text = "";
        public DynamicSpriteFont Font { get; set; }

        public string Text
        {
            get => text;
            set
            {
                text = value;
                RefreshDimensions();
            }
        }

        public UILabel()
        {
            InitializeFont();
            Text = "";
        }
        public UILabel(string text)
        {
            InitializeFont();
            Text = text;
        }

        private void InitializeFont()
        {
            Font = FontAssets.DeathText?.Value;
        }

        private void RefreshDimensions()
        {
            if (Text != null && Font != null)
            {
                Vector2 vector = Font.MeasureString(Text);
                Width = new StyleDimension(vector.X, 0);
                Height = new StyleDimension(vector.Y, 0);
            }
            else
            {
                Width = new StyleDimension(0, 0);
                Height = new StyleDimension(0, 0);
            }
        }

        //public override float Height
        //{
        //    get
        //    {
        //        if (height == 0f)
        //            return Font.MeasureString("Y").Y * Scale;
        //        return height * Scale;
        //    }
        //}

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = GetDimensions();
            var position = new Vector2(dimensions.X, dimensions.Y);
            if (Text != null && Font != null)
                spriteBatch.DrawString(Font, Text, position, ForegroundColor, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
        }
    }
}