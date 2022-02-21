using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.UI;

namespace Spellwright.UI.Components
{
    internal class UIImage : UIBase
    {
        private Rectangle? boundingRectangle = null;
        private Asset<Texture2D> imageTexture = null;
        private SpriteEffects SpriteEffect { get; set; }

        public UIImage()
        {
        }
        public UIImage(Asset<Texture2D> texture)
        {
            ImageTexture = texture;
        }
        private Asset<Texture2D> ImageTexture
        {
            get => imageTexture;
            set
            {
                imageTexture = value;
                UpdateDimensions();
            }
        }

        public Rectangle SourceRectangle
        {
            get
            {
                if (!boundingRectangle.HasValue)
                    boundingRectangle = new Rectangle?(default);
                return boundingRectangle.Value;
            }
            set
            {
                boundingRectangle = new Rectangle?(value);
                UpdateDimensions();
            }
        }

        private void UpdateDimensions()
        {
            float width;
            if (boundingRectangle.HasValue)
                width = boundingRectangle.Value.Width * Scale;
            else
                width = ImageTexture.Width() * Scale;
            Width = new StyleDimension(width, 0);

            float height;
            if (boundingRectangle.HasValue)
                height = boundingRectangle.Value.Height * Scale;
            else
                height = ImageTexture.Height() * Scale;
            Height = new StyleDimension(height, 0);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = GetDimensions();
            var position = new Vector2(dimensions.X, dimensions.Y);
            Texture2D texture = ImageTexture.Value;
            if (texture != null)
                spriteBatch.Draw(texture, position, boundingRectangle, ForegroundColor * Opacity, 0f, Vector2.Zero, Scale, SpriteEffect, 0f);
        }
    }
}