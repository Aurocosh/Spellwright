using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Spellwright.Common.Players;
using Spellwright.Content.Spells.Base;
using Spellwright.Core.Spells;
using Spellwright.UI.Components;
using Spellwright.UI.Components.Args;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace Spellwright.UI.States
{
    internal class UIMessageState : UIState
    {
        private readonly UIMessageBox messageBox = new("");
        private UIElement mainPanel;
        private UIScrollbar uIScrollbar;
        private UIElement buttonPanel;
        private UITextPanel<string> backButton;
        private UITextPanel<string> homeButton;
        private UITextPanel<string> closeButton;
        private UITextPanel<string> forwardButton;

        private readonly LinkedList<string> pageHistory;
        private LinkedListNode<string> currentPage;

        public UIMessageState()
        {
            pageHistory = new();
            currentPage = null;
        }

        public override void OnInitialize()
        {
            messageBox.BackgroundColor = new Color(60, 60, 60, 255) * 0.685f;
            messageBox.PaddingLeft = 10f;
            messageBox.PaddingTop = 10f;
            messageBox.PaddingRight = 10f;
            //messageBox.PaddingBottom = 10f;
            messageBox.PaddingBottom = 10f;
            messageBox.OnLineClicked += OnLineClicked;

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
                Width = { Pixels = 600 },
                Height = { Pixels = 60 },
                VAlign = 1f,
                HAlign = 0.5f,
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
        }

        public override void Update(GameTime gameTime)
        {
            homeButton.Left = new StyleDimension { Pixels = 120 };

            base.Update(gameTime);
            PlayerInput.LockVanillaMouseScroll("ModLoader/UIScrollbar");
            //if (uIScrollbar.ContainsPoint(Main.MouseScreen) || closeButton.ContainsPoint(Main.MouseScreen))
            //Main.LocalPlayer.mouseInterface = true;
            if (ContainsPoint(Main.MouseScreen))
                Main.LocalPlayer.mouseInterface = true;
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

        public void SetMessage(string text, bool resetHitory = true)
        {
            messageBox.SetText(text);

            if (resetHitory)
            {
                pageHistory.Clear();
            }
            else
            {
                while (pageHistory.Count > 0 && currentPage != pageHistory.Last)
                    pageHistory.RemoveLast();
            }

            pageHistory.AddLast(text);
            currentPage = pageHistory.Last;
            RefreshButtons();
        }

        private void SetMessage(LinkedListNode<string> setPage)
        {
            currentPage = setPage;
            messageBox.SetText(setPage.Value);
            RefreshButtons();
        }

        private void RefreshButtons()
        {
            if (currentPage == null || currentPage == pageHistory.First)
            {
                backButton.Left = new StyleDimension { Pixels = -500 };
            }
            else if (currentPage != null && currentPage != pageHistory.First)
            {
                backButton.Left = new StyleDimension { Pixels = 29 };
            }

            if (currentPage == null || currentPage == pageHistory.Last)
            {
                forwardButton.Left = new StyleDimension { Pixels = -500 };
            }
            else if (currentPage != null && currentPage != pageHistory.Last)
            {
                forwardButton.Left = new StyleDimension { Pixels = 320 };
            }
        }

        private void Close(UIMouseEvent evt, UIElement listeningElement)
        {
            Spellwright.Instance.userInterface.SetState(null);
        }

        private void OnHomeClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            var player = Main.LocalPlayer;
            string result = SpellInfoProvider.GetSpellList(player);
            SetMessage(result, true);
        }
        private void OnBackClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            if (currentPage?.Previous != null)
                SetMessage(currentPage.Previous);
        }
        private void OnForwardClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            if (currentPage?.Next != null)
                SetMessage(currentPage.Next);
        }

        private void OnLineClicked(object sender, LineClickEventArgs eventHandler)
        {
            string linkText = eventHandler.LinkText;
            if (linkText.Length == 0)
                return;

            var linkParts = linkText.Split(':', 2);
            if (linkParts.Length < 2)
                return;

            var type = linkParts[0].ToLower();
            if (type != "spell")
                return;

            var spellName = linkParts[1];
            var spell = SpellLibrary.GetSpellByName(spellName);
            if (spell == null)
                return;

            var player = Main.LocalPlayer;
            var spellPlayer = player.GetModPlayer<SpellwrightPlayer>();
            string fullMessage = SpellInfoProvider.GetSpellData(player, spellPlayer.PlayerLevel, spell, SpellData.EmptyData, true);
            SetMessage(fullMessage, false);
        }
    }
}
