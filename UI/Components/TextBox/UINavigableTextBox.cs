using Spellwright.UI.Components.Args;
using Spellwright.UI.Components.TextBox.TextData;
using Spellwright.UI.Components.TextBox.TextProcessors;
using System;
using System.Collections.Generic;
using Terraria.UI;

namespace Spellwright.UI.Components.TextBox
{
    internal class UINavigableTextBox : UIFormattedTextBox
    {
        private readonly LinkedList<PageStatus> pageHistory;
        private LinkedListNode<PageStatus> currentPage;
        private readonly ILinkProcessor linkProcessor;

        public event EventHandler<PageChangedEventArgs> OnPageChanged;

        public UINavigableTextBox(ILinkProcessor linkProcessor)
        {
            this.linkProcessor = linkProcessor;
            pageHistory = new LinkedList<PageStatus>();
            currentPage = null;
        }

        public string GetMessage()
        {
            return GetText();
        }

        public override void SetText(string text)
        {
            SetText(text, true);
        }

        public void SetText(string text, bool resetHitory = false)
        {
            if (resetHitory)
            {
                pageHistory.Clear();
            }
            else
            {
                while (pageHistory.Count > 0 && currentPage != pageHistory.Last)
                    pageHistory.RemoveLast();
            }

            pageHistory.AddLast(new PageStatus(text));
            currentPage = pageHistory.Last;
            base.SetText(text);
            OnPageChanged?.Invoke(this, new PageChangedEventArgs(text));
        }

        public void SetLink(string linkText, bool resetHitory = false)
        {
            if (resetHitory)
            {
                pageHistory.Clear();
                currentPage = null;
            }
            else
            {
                while (pageHistory.Count > 0 && currentPage != pageHistory.Last)
                    pageHistory.RemoveLast();
                if (pageHistory.Count == 0)
                    currentPage = null;
            }

            var linkResult = linkProcessor.Process(linkText);

            bool isPageRefresh = false;
            if (currentPage != null)
            {
                currentPage.Value.ScrollPosition = ViewPosition;
                var pageStatus = currentPage.Value;
                isPageRefresh = pageStatus.LinkId == linkResult.LinkId;
            }

            if (isPageRefresh)
            {
                var pageStatus = currentPage.Value;
                pageStatus.LinkText = linkResult.CorrectedLink;
                base.SetText(linkResult.Content);
            }
            else
            {
                pageHistory.AddLast(new PageStatus(linkResult.LinkId, linkResult.CorrectedLink, 0));
                currentPage = pageHistory.Last;
                base.SetText(linkResult.Content);
                OnPageChanged?.Invoke(this, new PageChangedEventArgs(linkResult.Content));
            }
        }

        private void SetPage(LinkedListNode<PageStatus> setPage)
        {
            if (currentPage != null)
                currentPage.Value.ScrollPosition = ViewPosition;

            currentPage = setPage;
            PageStatus pageStatus = setPage.Value;

            string text;
            if (pageStatus.IsLink)
            {
                var linkResult = linkProcessor.Process(pageStatus.LinkText);
                text = linkResult.Content;
            }
            else
            {
                text = pageStatus.PageText;
            }

            base.SetText(text);

            ViewPosition = pageStatus.ScrollPosition;
            OnPageChanged?.Invoke(this, new PageChangedEventArgs(text));
        }

        public void GoBack()
        {
            if (currentPage?.Previous != null)
                SetPage(currentPage.Previous);
        }

        public void GoForward()
        {
            if (currentPage?.Next != null)
                SetPage(currentPage.Next);
        }

        public void Refresh()
        {
            if (currentPage != null)
                SetPage(currentPage);
        }

        public bool CanGoBack()
        {
            return currentPage != null && currentPage != pageHistory.First;
        }

        public bool CanGoForward()
        {
            return currentPage != null && currentPage != pageHistory.Last;
        }

        public override void RightClick(UIMouseEvent evt)
        {
            base.RightClick(evt);
            GoBack();
        }
    }
}
