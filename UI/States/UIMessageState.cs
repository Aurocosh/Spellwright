using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Spellwright.UI.Components.Args;
using Spellwright.UI.Components.TextBox;
using Spellwright.UI.Components.TextBox.TextProcessors;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace Spellwright.UI.States
{
    internal class UIMessageState : UIState
    {
        private bool isTransparent = false;
        private readonly UINavigableTextBox messageBox;
        private UIElement mainPanel;
        private UIScrollbar uIScrollbar;
        private UIPanel buttonPanel;

        private UITextPanel<string> backButton;
        private UITextPanel<string> homeButton;
        private UITextPanel<string> closeButton;
        private UITextPanel<string> forwardButton;
        private UITextPanel<string> transparancyButton;

        Color solidBackground = new(60, 60, 60, 255);
        Color brightBackground = Color.DarkGoldenrod * 0.685f;
        Color transparentBackground = new Color(60, 60, 60, 255) * 0.685f;

        public UIMessageState()
        {
            messageBox = new UINavigableTextBox(new SpellLinkProcessor());
        }

        public override void OnInitialize()
        {
            messageBox.BackgroundColor = solidBackground;
            messageBox.PaddingLeft = 20f;
            messageBox.PaddingTop = 10f;
            messageBox.PaddingRight = 20f;
            messageBox.PaddingBottom = 50f;
            messageBox.OnLinkClicked += OnLineClicked;
            messageBox.OnPageChanged += OnPageChanged;

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

            uIScrollbar = new UIScrollbar
            {
                Height = { Pixels = -20, Percent = 1f },
                VAlign = 0.5f,
                HAlign = 1f,
                MarginRight = 5f
            }.WithView(100f, 1000f);
            mainPanel.Append(uIScrollbar);

            var customTexture = Spellwright.Instance.Assets.Request<Texture2D>("UI/Images/Scrollbar", AssetRequestMode.ImmediateLoad);
            var prop = uIScrollbar.GetType().GetField("_texture", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            prop.SetValue(uIScrollbar, customTexture);

            messageBox.SetScrollbar(uIScrollbar);

            buttonPanel = new UIPanel
            {
                Width = { Pixels = 460 },
                Height = { Pixels = 60 },
                VAlign = 1f,
                HAlign = 0.5f,
                MarginBottom = 3f,
                BackgroundColor = transparentBackground
            };
            mainPanel.Append(buttonPanel);

            backButton = new UITextPanel<string>("<--", 0.7f, true)
            {
                //Width = { Pixels = -10, Percent = 1f / 3f },
                MinWidth = { Pixels = 90 },
                MinHeight = { Pixels = 40 },
                Left = { Pixels = 19 },
                VAlign = 0.5f,
                BackgroundColor = transparentBackground
            };
            backButton.WithFadedMouseOver(brightBackground, transparentBackground);
            backButton.OnClick += OnBackClicked;
            buttonPanel.Append(backButton);

            homeButton = new UITextPanel<string>("Home", 0.7f, true)
            {
                //Width = { Pixels = -10, Percent = 1f / 3f },
                MinWidth = { Pixels = 90 },
                MinHeight = { Pixels = 40 },
                Left = { Pixels = 120 },
                VAlign = 0.5f,
                BackgroundColor = transparentBackground
            };
            homeButton.WithFadedMouseOver(brightBackground, transparentBackground);
            homeButton.OnClick += OnHomeClicked;
            buttonPanel.Append(homeButton);

            closeButton = new UITextPanel<string>("Close", 0.7f, true)
            {
                //Width = { Pixels = -10, Percent = 1f / 3f },
                MinWidth = { Pixels = 90 },
                MinHeight = { Pixels = 40 },
                Left = { Pixels = 220 },
                VAlign = 0.5f,
                BackgroundColor = transparentBackground
            };
            closeButton.WithFadedMouseOver(brightBackground, transparentBackground);
            closeButton.OnClick += Close;
            buttonPanel.Append(closeButton);

            forwardButton = new UITextPanel<string>("-->", 0.7f, true)
            {
                //Width = { Pixels = -10, Percent = 1f / 3f },
                MinWidth = { Pixels = 90 },
                MinHeight = { Pixels = 40 },
                Left = { Pixels = 330 },
                VAlign = 0.5f,
                BackgroundColor = transparentBackground
            };
            forwardButton.WithFadedMouseOver(brightBackground, transparentBackground);
            forwardButton.OnClick += OnForwardClicked;
            buttonPanel.Append(forwardButton);

            transparancyButton = new UITextPanel<string>("Tr", 0.7f, false)
            {
                MinWidth = { Pixels = 20 },
                MinHeight = { Pixels = 20 },
                VAlign = 1f,
                HAlign = 1f,
                MarginBottom = 5f,
                MarginRight = 30f,
                BackgroundColor = transparentBackground
            };
            transparancyButton.WithFadedMouseOver(brightBackground, transparentBackground);
            transparancyButton.OnClick += OnTransparancyClicked;
            mainPanel.Append(transparancyButton);

            Append(mainPanel);
            RefreshButtons();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            PlayerInput.LockVanillaMouseScroll("ModLoader/UIScrollbar");
            if (ContainsPoint(Main.MouseScreen))
                Main.LocalPlayer.mouseInterface = true;

            buttonPanel.BackgroundColor = new Color(150, 150, 150, 255) * 0.685f;
            buttonPanel.BorderColor = Color.Black;
        }

        public override void OnActivate()
        {
            base.OnActivate();
            messageBox.Refresh();
        }

        public bool HasText()
        {
            return messageBox.HasText();
        }

        public string GetText()
        {
            return messageBox.GetText();
        }

        public void SetText(string text, bool resetHitory = false) => messageBox.SetText(text, resetHitory);
        public void SetLink(string linkText, bool resetHitory = false) => messageBox.SetLink(linkText, resetHitory);
        public void GoHome() => messageBox.SetLink("link:Static=id:Home", true);
        public void Refresh() => messageBox.Refresh();

        private void RefreshButtons()
        {
            if (messageBox.CanGoBack())
                backButton.Left = new StyleDimension { Pixels = 19 };
            else
                backButton.Left = new StyleDimension { Pixels = -3000 };

            if (messageBox.CanGoForward())
                forwardButton.Left = new StyleDimension { Pixels = 330 };
            else
                forwardButton.Left = new StyleDimension { Pixels = -3000 };
        }

        private void Close(UIMouseEvent evt, UIElement listeningElement)
        {
            Spellwright.Instance.userInterface.SetState(null);
        }

        private void OnTransparancyClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            isTransparent = !isTransparent;
            messageBox.BackgroundColor = isTransparent ? transparentBackground : solidBackground;
        }

        private void OnHomeClicked(UIMouseEvent evt, UIElement listeningElement) => GoHome();

        private void OnBackClicked(UIMouseEvent evt, UIElement listeningElement) => messageBox.GoBack();

        private void OnForwardClicked(UIMouseEvent evt, UIElement listeningElement) => messageBox.GoForward();

        private void OnLineClicked(object sender, LinkClickedEventArgs eventHandler) => messageBox.SetLink(eventHandler.Link);

        private void OnPageChanged(object sender, PageChangedEventArgs eventArgs) => RefreshButtons();
    }
}
