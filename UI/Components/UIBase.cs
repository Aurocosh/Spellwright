using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;

namespace Spellwright.UI.Components
{
    internal class UIBase : UIElement
    {
        public Color ForegroundColor { get; set; } = Color.White;
        public Color BackgroundColor { get; set; } = Color.White;

        public float Scale { get; set; } = 1f;
        public bool Visible { get; set; } = true;

        public UIBase()
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Update();
        }
        public virtual void Update()
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Visible)
                base.Draw(spriteBatch);
        }
    }
}