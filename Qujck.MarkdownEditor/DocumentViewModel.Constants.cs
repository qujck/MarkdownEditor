﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Qujck.MarkdownEditor.Infrastructure;

namespace Qujck.MarkdownEditor
{
    public sealed partial class DocumentViewModel
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
                    { BottomRowHeight, ON },
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