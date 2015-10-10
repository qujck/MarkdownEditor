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
    public static partial class Constants
    {
        public struct DocumentViewModel
        {
            const string On = "1*";
            const string Off = "0";
            const string BorderOff = "0,0,0,0";
            const string BorderRight = "0,0,1,0";
            const string BorderBottom = "0,0,0,1";

            public const string LeftColumnWidth = "LeftColumnWidth";
            public const string RightColumnWidth = "RightColumnWidth";
            public const string TopRowHeight = "TopRowHeight";
            public const string BottomRowHeight = "BottomRowHeight";
            public const string TextEditorColumn = "TextEditorColumn";
            public const string TextEditorRow = "TextEditorRow";
            public const string RenderedViewColumn = "RenderedViewColumn";
            public const string RenderedViewRow = "RenderedViewRow";
            public const string BorderThickness = "BorderThickness";
            public const string CurrentView = "CurrentView";
            public const string OpeningText = "OpeningText";
            public const string CurrentText = "CurrentText";
            public const string FilePath = "FileName";

            public static readonly IDictionary<string, object>[] Views =
                new IDictionary<string, object>[]
                {
                    VerticalView(),
                    HorizontalView(),
                    TextEditorView(),
                    RenderedViewView(),
                };

            private static IDictionary<string, object> VerticalView()
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
                    { BorderThickness, BorderRight }
                };
            }

            private static IDictionary<string, object> HorizontalView()
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
                    { BorderThickness, BorderBottom }
                };
            }

            private static IDictionary<string, object> TextEditorView()
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
                    { BorderThickness, BorderOff }
                };
            }

            private static IDictionary<string, object> RenderedViewView()
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
                    { BorderThickness, BorderOff }
                };
            }
        }
    }
}