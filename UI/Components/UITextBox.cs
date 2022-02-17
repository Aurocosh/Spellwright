using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;

namespace Spellwright.UI.Components
{
    internal class UITextBox : UIBase
    {
        private RasterizerState _rasterizerState = new() { ScissorTestEnable = true };
        internal static Asset<Texture2D> textboxBackground;
        private static Texture2D textboxFill;

        private static Texture2D TextboxFill
        {
            get
            {
                if (textboxFill == null && textboxBackground.Value != null)
                {
                    int width = textboxBackground.Width();
                    int height = textboxBackground.Height();
                    var edgeColors = new Color[width * height];
                    textboxBackground.Value.GetData(edgeColors);
                    var fillColors = new Color[height];
                    for (int y = 0; y < fillColors.Length; y++)
                        fillColors[y] = edgeColors[width - 1 + y * width];
                    textboxFill = new Texture2D(Main.graphics.GraphicsDevice, 1, fillColors.Length);
                    textboxFill.SetData(fillColors);
                }
                return textboxFill;
            }
        }

        private bool focused = false;
        private static float blinkTime = 1f;
        private static float timer = 0f;

        public delegate void KeyPressedHandler(object sender, char key);

        public event EventHandler OnTabPressed;
        public event EventHandler OnEscPressed;
        public event EventHandler OnEnterPresseed;
        public event KeyPressedHandler OnKeyPressed;

        private bool drawCarrot = true;
        private UILabel label = new UILabel();

        public string Text { get; set; } = "";

        public int MaxCharacters { get; set; } = 20;

        public UITextBox()
        {
            textboxBackground = Spellwright.instance.Assets.Request<Texture2D>("UI/Components/TextboxEdge", AssetRequestMode.ImmediateLoad);

            label.ForegroundColor = Color.Black;
            label.Scale = Height / label.Height;
            label.Position = new Vector2(4, 4);
            AddChild(label);

            width = 400;
        }

        public void Focus()
        {
            if (!focused)
            {
                Main.clrInput();
                focused = true;
                Main.blockInput = true;
                timer = 0f;
            }
        }

        public void Unfocus()
        {
            if (focused)
            {
                focused = false;
                Main.blockInput = false;
            }
        }

        public override float Width
        {
            get { return width; }
            //set { width = value; }
        }

        public override float Height
        {
            get { return textboxBackground.Height(); }
            //set { height = value; }
        }

        public override void Update()
        {
            base.Update();

            if (focused)
            {
                timer += .1f;
                if (timer < blinkTime / 2)
                    drawCarrot = true;
                else
                    drawCarrot = false;
                if (timer >= blinkTime)
                    timer = 0;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (focused)
            {
                Terraria.GameInput.PlayerInput.WritingText = true;
                Main.instance.HandleIME();
                string oldText = Text;
                Text = Main.GetInputText(Text);
                if (oldText != Text)
                    OnKeyPressed?.Invoke(this, ' ');
                if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Tab))
                    OnTabPressed?.Invoke(this, new EventArgs());
                if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
                    OnEscPressed?.Invoke(this, new EventArgs());
                if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter))
                    OnEnterPresseed?.Invoke(this, new EventArgs());
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Main.UIScaleMatrix);
                Main.instance.DrawWindowsIMEPanel(new Vector2(98f, Main.screenHeight - 36), 0f);
            }

            spriteBatch.Draw(textboxBackground.Value, AbsolutePosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            int fillWidth = (int)Width - 2 * textboxBackground.Width();
            Vector2 pos = AbsolutePosition;
            pos.X += textboxBackground.Width();
            if (TextboxFill != null)
                spriteBatch.Draw(TextboxFill, pos, null, Color.White, 0f, Vector2.Zero, new Vector2(fillWidth, 1f), SpriteEffects.None, 0f);
            pos.X += fillWidth;
            spriteBatch.Draw(textboxBackground.Value, pos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0f);
            string drawString = Text;
            if (drawCarrot && focused)
                drawString += "|";
            label.Text = drawString;

            pos = AbsolutePosition;

            if (pos.X <= Main.screenWidth && pos.Y <= Main.screenHeight && pos.X + Width >= 0 && pos.Y + Height >= 0)
            {
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, _rasterizerState, null, Main.UIScaleMatrix);

                var cutRect = new Rectangle((int)pos.X, (int)pos.Y, (int)Width, (int)Height);
                cutRect = GetClippingRectangle(spriteBatch, cutRect);
                Rectangle currentRect = spriteBatch.GraphicsDevice.ScissorRectangle;
                spriteBatch.GraphicsDevice.ScissorRectangle = cutRect;

                base.Draw(spriteBatch);

                spriteBatch.GraphicsDevice.ScissorRectangle = currentRect;
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, null, null, null, null, Main.UIScaleMatrix);
            }
        }
        public static Rectangle GetClippingRectangle(SpriteBatch spriteBatch, Rectangle r)
        {
            var vector = new Vector2(r.X, r.Y);
            Vector2 position = new Vector2(r.Width, r.Height) + vector;
            vector = Vector2.Transform(vector, Main.UIScaleMatrix);
            position = Vector2.Transform(position, Main.UIScaleMatrix);
            var result = new Rectangle((int)vector.X, (int)vector.Y, (int)(position.X - vector.X), (int)(position.Y - vector.Y));
            int width = spriteBatch.GraphicsDevice.Viewport.Width;
            int height = spriteBatch.GraphicsDevice.Viewport.Height;
            result.X = Utils.Clamp(result.X, 0, width);
            result.Y = Utils.Clamp(result.Y, 0, height);
            result.Width = Utils.Clamp(result.Width, 0, width - result.X);
            result.Height = Utils.Clamp(result.Height, 0, height - result.Y);
            return result;
        }
    }
}