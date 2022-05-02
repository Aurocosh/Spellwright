using Spellwright.UI.Components.Args;
using Spellwright.UI.Components.TextBox.TextData;
using System;
using System.Collections.Generic;

namespace Spellwright.UI.Components.TextBox
{
    internal class UINavigableTextBox : UIFancyTextBox
    {
        private readonly LinkedList<PageStatus> pageHistory;
        private LinkedListNode<PageStatus> currentPage;

        public event EventHandler<PageChangedEventArgs> OnPageChanged;

        public UINavigableTextBox()
        {
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

        public void SetText(string text, bool resetHitory)
        {
            if (currentPage != null)
                currentPage.Value.ScrollPosition = ViewPosition;

            if (resetHitory)
            {
                pageHistory.Clear();
            }
            else
            {
                while (pageHistory.Count > 0 && currentPage != pageHistory.Last)
                    pageHistory.RemoveLast();
            }

            pageHistory.AddLast(new PageStatus(text, 0));
            currentPage = pageHistory.Last;
            base.SetText(text);

            OnPageChanged?.Invoke(this, new PageChangedEventArgs(text));
        }

        private void SetPage(LinkedListNode<PageStatus> setPage)
        {
            if (currentPage != null)
                currentPage.Value.ScrollPosition = ViewPosition;

            currentPage = setPage;
            PageStatus pageStatus = setPage.Value;
            base.SetText(pageStatus.Text);

            ViewPosition = pageStatus.ScrollPosition;
            OnPageChanged?.Invoke(this, new PageChangedEventArgs(pageStatus.Text));
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

        public bool CanGoBack()
        {
            return currentPage != null && currentPage != pageHistory.First;
        }

        public bool CanGoForward()
        {
            return currentPage != null && currentPage != pageHistory.Last;
        }
    }
}
