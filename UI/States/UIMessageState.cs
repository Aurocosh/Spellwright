﻿using Microsoft.Xna.Framework;
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
        private readonly UINavigableTextBox messageBox;
        private UIElement mainPanel;
        private UIScrollbar uIScrollbar;
        private UIElement buttonPanel;
        private UITextPanel<string> backButton;
        private UITextPanel<string> homeButton;
        private UITextPanel<string> closeButton;
        private UITextPanel<string> forwardButton;

        public UIMessageState()
        {
            messageBox = new UINavigableTextBox(new SpellLinkProcessor());
        }

        public override void OnInitialize()
        {
            messageBox.BackgroundColor = new Color(60, 60, 60, 255) * 0.685f;
            messageBox.PaddingLeft = 10f;
            messageBox.PaddingTop = 10f;
            messageBox.PaddingRight = 10f;
            //messageBox.PaddingBottom = 10f;
            messageBox.PaddingBottom = 10f;
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
                HAlign = 1f
            }.WithView(100f, 1000f);
            mainPanel.Append(uIScrollbar);

            var customTexture = Spellwright.Instance.Assets.Request<Texture2D>("UI/Images/Scrollbar", AssetRequestMode.ImmediateLoad);
            var prop = uIScrollbar.GetType().GetField("_texture", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            prop.SetValue(uIScrollbar, customTexture);

            messageBox.SetScrollbar(uIScrollbar);

            buttonPanel = new UIElement
            {
                Width = { Pixels = 500 },
                Height = { Pixels = 60 },
                VAlign = 1f,
                //HAlign = 0.5f,
                HAlign = 1f,

            };
            mainPanel.Append(buttonPanel);

            backButton = new UITextPanel<string>("<--", 0.7f, true)
            {
                //Width = { Pixels = -10, Percent = 1f / 3f },
                MinWidth = { Pixels = 90 },
                MinHeight = { Pixels = 40 },
                Left = { Pixels = 29 },
                Top = { Pixels = 10 },
                //Top = { Pixels = -30 },
                BackgroundColor = new Color(60, 60, 60, 255) * 0.685f
            };
            backButton.WithFadedMouseOver(new Color(120, 120, 120, 255) * 0.685f, new Color(60, 60, 60, 255) * 0.685f);
            backButton.OnClick += OnBackClicked;
            buttonPanel.Append(backButton);

            homeButton = new UITextPanel<string>("Home", 0.7f, true)
            {
                //Width = { Pixels = -10, Percent = 1f / 3f },
                MinWidth = { Pixels = 90 },
                MinHeight = { Pixels = 40 },
                Left = { Pixels = 120 },
                Top = { Pixels = 10 },
                //Top = { Pixels = -30 },
                BackgroundColor = new Color(60, 60, 60, 255) * 0.685f
            };
            homeButton.WithFadedMouseOver(new Color(120, 120, 120, 255) * 0.685f, new Color(60, 60, 60, 255) * 0.685f);
            homeButton.OnClick += OnHomeClicked;
            buttonPanel.Append(homeButton);

            closeButton = new UITextPanel<string>("Close", 0.7f, true)
            {
                //Width = { Pixels = -10, Percent = 1f / 3f },
                MinWidth = { Pixels = 90 },
                MinHeight = { Pixels = 40 },
                Left = { Pixels = 220 },
                Top = { Pixels = 10 },
                //Top = { Pixels = -30 },
                BackgroundColor = new Color(60, 60, 60, 255) * 0.685f
            };
            closeButton.WithFadedMouseOver(new Color(120, 120, 120, 255) * 0.685f, new Color(60, 60, 60, 255) * 0.685f);
            closeButton.OnClick += Close;
            buttonPanel.Append(closeButton);

            forwardButton = new UITextPanel<string>("-->", 0.7f, true)
            {
                //Width = { Pixels = -10, Percent = 1f / 3f },
                MinWidth = { Pixels = 90 },
                MinHeight = { Pixels = 40 },
                Left = { Pixels = 320 },
                Top = { Pixels = 10 },
                //Top = { Pixels = -30 },
                BackgroundColor = new Color(60, 60, 60, 255) * 0.685f
            };
            forwardButton.WithFadedMouseOver(new Color(120, 120, 120, 255) * 0.685f, new Color(60, 60, 60, 255) * 0.685f);
            forwardButton.OnClick += OnForwardClicked;
            buttonPanel.Append(forwardButton);

            Append(mainPanel);
            RefreshButtons();
        }

        public override void Update(GameTime gameTime)
        {
            homeButton.Left = new StyleDimension { Pixels = 120 };

            base.Update(gameTime);
            PlayerInput.LockVanillaMouseScroll("ModLoader/UIScrollbar");
            if (ContainsPoint(Main.MouseScreen))
                Main.LocalPlayer.mouseInterface = true;
        }

        public override void OnActivate()
        {
            base.OnActivate();
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
        public void GoHome()
        {
            messageBox.SetLink("link:Static=id:Home", true);
        }

        private void RefreshButtons()
        {
            if (messageBox.CanGoBack())
                backButton.Left = new StyleDimension { Pixels = 29 };
            else
                backButton.Left = new StyleDimension { Pixels = -3000 };

            if (messageBox.CanGoForward())
                forwardButton.Left = new StyleDimension { Pixels = 320 };
            else
                forwardButton.Left = new StyleDimension { Pixels = -3000 };
        }

        private void Close(UIMouseEvent evt, UIElement listeningElement)
        {
            Spellwright.Instance.userInterface.SetState(null);
        }

        private void OnHomeClicked(UIMouseEvent evt, UIElement listeningElement) => GoHome();

        private void OnBackClicked(UIMouseEvent evt, UIElement listeningElement) => messageBox.GoBack();

        private void OnForwardClicked(UIMouseEvent evt, UIElement listeningElement) => messageBox.GoForward();

        private void OnLineClicked(object sender, LinkClickedEventArgs eventHandler) => messageBox.SetLink(eventHandler.Link);

        private void OnPageChanged(object sender, PageChangedEventArgs eventArgs) => RefreshButtons();
    }
}
