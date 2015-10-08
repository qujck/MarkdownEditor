using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Qujck.MarkdownEditor.Infrastructure;

namespace Qujck.MarkdownEditor
{
    public sealed class DocumentViewModel : AbstractViewModel
    {
        const string ON = "1*";
        const string OFF = "0";

        const string LeftColumnWidth = "LeftColumnWidth";
        const string RightColumnWidth = "RightColumnWidth";
        const string TopRowHeight = "TopRowHeight";
        const string BottomRowHeight = "BottomRowHeight";
        const string TextEditorColumn = "TextEditorColumn";
        const string TextEditorRow = "TextEditorRow";
        const string RenderedViewColumn = "RenderedViewColumn";
        const string RenderedViewRow = "RenderedViewRow";
        const string CurrentView = "CurrentView";

        private enum View
        {
            Vertical,
            Horizontal,
            TextEditor,
            RenderedView
        }

        public DocumentViewModel() : base(VerticalView)
        {
        }

        public void Next()
        {
            this.Update(WhatsNext((View)this[CurrentView]));
        }

        public void Previous()
        {
            this.Update(WhatsPrevious((View)this[CurrentView]));
        }

        private static IDictionary<string, object> WhatsNext(View currentView)
        {
            switch (currentView)
            {
                case View.Vertical:
                    return HorizontalView;
                case View.Horizontal:
                    return TextEditorView;
                case View.TextEditor:
                    return RenderedViewView;
                case View.RenderedView:
                    return VerticalView;
                default:
                    throw new InvalidOperationException();
            }
        }

        private static IDictionary<string, object> WhatsPrevious(View currentView)
        {
            switch (currentView)
            {
                case View.RenderedView:
                    return TextEditorView;
                case View.TextEditor:
                    return HorizontalView;
                case View.Horizontal:
                    return VerticalView;
                case View.Vertical:
                    return RenderedViewView;
                default:
                    throw new InvalidOperationException();
            }
        }

        private static IDictionary<string, object> VerticalView
        {
            get
            {
                return new Dictionary<string, object>
                {
                    { LeftColumnWidth, ON },
                    { RightColumnWidth, ON },
                    { TopRowHeight, ON },
                    { BottomRowHeight, OFF },
                    { TextEditorColumn, 0 },
                    { TextEditorRow, 0 },
                    { RenderedViewColumn, 1 },
                    { RenderedViewRow, 0 },
                    { CurrentView, View.Vertical }
                };
            }
        }

        private static IDictionary<string, object> HorizontalView
        {
            get
            {
                return new Dictionary<string, object>
                {
                    { LeftColumnWidth, ON },
                    { RightColumnWidth, OFF },
                    { TopRowHeight, ON },
                    { BottomRowHeight, OFF },
                    { TextEditorColumn, 0 },
                    { TextEditorRow, 0 },
                    { RenderedViewColumn, 0 },
                    { RenderedViewRow, 1 },
                    { CurrentView, View.Horizontal }
                };
            }
        }

        private static IDictionary<string, object> TextEditorView
        {
            get
            {
                return new Dictionary<string, object>
                {
                    { LeftColumnWidth, ON },
                    { RightColumnWidth, OFF },
                    { TopRowHeight, ON },
                    { BottomRowHeight, OFF },
                    { TextEditorColumn, 0 },
                    { TextEditorRow, 0 },
                    { RenderedViewColumn, 1 },
                    { RenderedViewRow, 0 },
                    { CurrentView, View.TextEditor }
                };
            }
        }

        private static IDictionary<string, object> RenderedViewView
        {
            get
            {
                return new Dictionary<string, object>
                {
                    { LeftColumnWidth, OFF },
                    { RightColumnWidth, ON },
                    { TopRowHeight, ON },
                    { BottomRowHeight, OFF },
                    { TextEditorColumn, 0 },
                    { TextEditorRow, 0 },
                    { RenderedViewColumn, 1 },
                    { RenderedViewRow, 0 },
                    { CurrentView, View.RenderedView }
                };
            }
        }
    }
}