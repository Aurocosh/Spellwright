using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;

namespace Spellwright.UI.Components
{
    internal class UIBase
    {
        protected float width;
        protected float height;

        private readonly List<UIBase> children = new();

        public UIBase Parent { get; set; }
        public Vector2 Position { get; set; } = Vector2.Zero;
        public Vector2 AbsolutePosition => Parent != null ? Parent.AbsolutePosition + Position : Position;

        public virtual float Width
        {
            get { return width; }
            set { width = value; }
        }
        public virtual float Height
        {
            get { return height; }
            set { height = value; }
        }

        public Color ForegroundColor { get; set; } = Color.White;
        public Color BackgroundColor { get; set; } = Color.White;

        public float Scale { get; set; } = 1f;
        public float Opacity { get; set; } = 1f;
        public bool Visible { get; set; } = true;

        public virtual void Update()
        {
        }

        public UIBase()
        {
        }

        public void CenterToParent()
        {
            var pos = Parent != null ? new Vector2(Width, Height) : new Vector2(Main.screenWidth, Main.screenHeight);
            pos.X = (pos.X - Width) / 2f;
            pos.Y = (pos.Y - Height) / 2f;
            Position = pos;
        }

        public virtual void AddChild(UIBase view)
        {
            view.Parent = this;
            children.Add(view);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible)
                return;

            int childCount = children.Count;
            for (int i = 0; i < childCount; i++)
            {
                if (childCount != children.Count)
                    return;
                UIBase uIView = children[i];
                if (uIView.Visible)
                    uIView.Draw(spriteBatch);
            }
        }

    }
}