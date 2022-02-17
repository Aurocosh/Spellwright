using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;

namespace Spellwright.UI.Components
{
    internal class UIImage : UIBase
    {
        private Rectangle? boundingRectangle = null;
        private SpriteEffects SpriteEffect { get; set; }
        private Asset<Texture2D> ImageTexture { get; set; }

        public UIImage()
        {
        }
        public UIImage(Asset<Texture2D> texture)
        {
            ImageTexture = texture;
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
            }
        }

        public override float Width
        {
            get
            {
                if (boundingRectangle.HasValue)
                    return boundingRectangle.Value.Width * Scale;
                return ImageTexture.Width() * Scale;
            }
        }

        public override float Height
        {
            get
            {
                if (boundingRectangle.HasValue)
                    return boundingRectangle.Value.Height * Scale;
                return ImageTexture.Height() * Scale;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Visible)
            {
                Texture2D texture = ImageTexture.Value;
                if (texture != null)
                    spriteBatch.Draw(texture, AbsolutePosition, boundingRectangle, ForegroundColor * Opacity, 0f, Vector2.Zero, Scale, SpriteEffect, 0f);
            }
            base.Draw(spriteBatch);
        }
    }
}