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
    public sealed partial class DocumentViewModel
    {
        const string On = "1*";
        const string Off = "0";
        const string BorderOff = "0,0,0,0";
        const string BorderRight = "0,0,1,0";
        const string BorderBottom = "0,0,0,1";

        const string LeftColumnWidth = "LeftColumnWidth";
        const string RightColumnWidth = "RightColumnWidth";
        const string TopRowHeight = "TopRowHeight";
        const string BottomRowHeight = "BottomRowHeight";
        const string TextEditorColumn = "TextEditorColumn";
        const string TextEditorRow = "TextEditorRow";
        const string RenderedViewColumn = "RenderedViewColumn";
        const string RenderedViewRow = "RenderedViewRow";
        const string BorderThickness = "BorderThickness";
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
                    { LeftColumnWidth, On },
                    { RightColumnWidth, On },
                    { TopRowHeight, On },
                    { BottomRowHeight, Off },
                    { TextEditorColumn, 0 },
                    { TextEditorRow, 0 },
                    { RenderedViewColumn, 1 },
                    { RenderedViewRow, 0 },
                    { BorderThickness, BorderRight },
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
                    { LeftColumnWidth, On },
                    { RightColumnWidth, Off },
                    { TopRowHeight, On },
                    { BottomRowHeight, On },
                    { TextEditorColumn, 0 },
                    { TextEditorRow, 0 },
                    { RenderedViewColumn, 0 },
                    { RenderedViewRow, 1 },
                    { BorderThickness, BorderBottom },
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
                    { LeftColumnWidth, On },
                    { RightColumnWidth, Off },
                    { TopRowHeight, On },
                    { BottomRowHeight, Off },
                    { TextEditorColumn, 0 },
                    { TextEditorRow, 0 },
                    { RenderedViewColumn, 1 },
                    { RenderedViewRow, 0 },
                    { BorderThickness, BorderOff },
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
                    { LeftColumnWidth, Off },
                    { RightColumnWidth, On },
                    { TopRowHeight, On },
                    { BottomRowHeight, Off },
                    { TextEditorColumn, 0 },
                    { TextEditorRow, 0 },
                    { RenderedViewColumn, 1 },
                    { RenderedViewRow, 0 },
                    { BorderThickness, BorderOff },
                    { CurrentView, View.RenderedView }
                };
            }
        }
    }
}