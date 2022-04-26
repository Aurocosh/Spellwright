using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Spellwright.UI.Components;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace Spellwright.UI.States
{
    internal class UIMessageState : UIState
    {
        private readonly UIMessageBox messageBox = new("");
        private UIElement mainPanel;

        public override void OnInitialize()
        {
            messageBox.BackgroundColor = new Color(60, 60, 60, 255) * 0.685f;
            messageBox.PaddingLeft = 10f;
            messageBox.PaddingTop = 10f;
            messageBox.PaddingRight = 10f;
            messageBox.PaddingBottom = 10f;

            mainPanel = new UIElement
            {
                Width = { Percent = 0.8f },
                Top = { Pixels = 200 },
                Height = { Pixels = -240, Percent = 1f },
                HAlign = 0.5f
            };

            messageBox.Width.Percent = 1f;
            messageBox.Height.Percent = .8f;
            messageBox.HAlign = 0.5f;
            mainPanel.Append(messageBox);

            var uIScrollbar = new UIScrollbar
            {
                Height = { Pixels = -20, Percent = 1f },
                VAlign = 0.5f,
                HAlign = 1f
            }.WithView(100f, 1000f);
            mainPanel.Append(uIScrollbar);

            var customTexture = Spellwright.Instance.Assets.Request<Texture2D>("UI/Images/Scrollbar", AssetRequestMode.ImmediateLoad);
            var prop = uIScrollbar.GetType().GetField("_texture", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            prop.SetValue(uIScrollbar, customTexture);

            messageBox.SetScrollbar(uIScrollbar);

            var closeButton = new UITextPanel<string>("Close", 0.7f, true)
            {
                Width = { Pixels = -10, Percent = 1f / 3f },
                Height = { Pixels = 50 },
                VAlign = 1f,
                HAlign = 0.5f,
                //Top = { Pixels = -30 },
                BackgroundColor = new Color(60, 60, 60, 255) * 0.685f
            };
            closeButton.WithFadedMouseOver(new Color(120, 120, 120, 255) * 0.685f, new Color(60, 60, 60, 255) * 0.685f);
            closeButton.OnClick += Close;
            mainPanel.Append(closeButton);

            Append(mainPanel);
        }

        public override void OnActivate()
        {
            base.OnActivate();
        }
        public bool HasMessage()
        {
            return messageBox.HasText();
        }

        public string GetMessage()
        {
            return messageBox.GetText();
        }

        public void SetMessage(string text)
        {
            messageBox.SetText(text);
        }

        private void Close(UIMouseEvent evt, UIElement listeningElement)
        {
            Spellwright.Instance.userInterface.SetState(null);
        }
    }
}
