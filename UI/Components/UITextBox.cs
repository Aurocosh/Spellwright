using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.GameInput;
using Terraria.UI;

namespace Spellwright.UI.Components
{
    internal class UITextBox : UIBase
    {
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

        public delegate void TextChangeHandler(object sender, string text);

        public event EventHandler OnTabPressed;
        public event EventHandler OnEscPressed;
        public event EventHandler OnEnterPresseed;
        public event TextChangeHandler OnKeyPressed;

        private bool drawCarrot = true;
        private UILabel label = new UILabel();

        public string Text { get; set; } = "";

        public int MaxCharacters { get; set; } = 20;

        public UITextBox()
        {
            textboxBackground = Spellwright.Instance.Assets.Request<Texture2D>("UI/Images/TextboxEdge", AssetRequestMode.ImmediateLoad);

            label.ForegroundColor = Color.Black;
            label.Scale = 0.4f;
            label.Left = new StyleDimension(4, 0);
            label.Top = new StyleDimension(4, 0);
            Append(label);

            var sdf = textboxBackground.Height();
            Height = new StyleDimension(sdf, 0);
            Width = new StyleDimension(400, 0);
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
            CalculatedStyle dimensions = GetDimensions();
            CalculatedStyle labelDimensions = label.GetDimensions();

            label.Scale = dimensions.Height / labelDimensions.Height;
            label.Scale *= 0.4f;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = GetDimensions();
            var point = new Point((int)dimensions.X, (int)dimensions.Y);
            var position = new Vector2(dimensions.X, dimensions.Y);

            if (focused)
            {
                PlayerInput.WritingText = true;
                Main.CurrentInputTextTakerOverride = this;
                Main.instance.HandleIME();
                string oldText = Text;
                Text = Main.GetInputText(Text);
                if (oldText != Text)
                    OnKeyPressed?.Invoke(this, Text);
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

            spriteBatch.Draw(textboxBackground.Value, position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            int fillWidth = (int)dimensions.Width - 2 * textboxBackground.Width();
            Vector2 pos = position;
            pos.X += textboxBackground.Width();
            if (TextboxFill != null)
                spriteBatch.Draw(TextboxFill, pos, null, Color.White, 0f, Vector2.Zero, new Vector2(fillWidth, 1f), SpriteEffects.None, 0f);
            pos.X += fillWidth;
            spriteBatch.Draw(textboxBackground.Value, pos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0f);
            string drawString = Text;
            if (drawCarrot && focused)
                drawString += "|";
            label.Text = drawString;
        }
    }
}