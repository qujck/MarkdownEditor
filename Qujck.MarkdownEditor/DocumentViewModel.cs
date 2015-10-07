using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Qujck.MarkdownEditor
{
    public sealed class DocumentViewModel : AbstractDynamicObject
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

        private enum View
        {
            Vertical,
            Horizontal,
            TextEditor,
            RenderedView
        }

        private View current;

        public DocumentViewModel() : base(
            LeftColumnWidth,
            RightColumnWidth,
            TopRowHeight,
            BottomRowHeight,
            TextEditorColumn,
            TextEditorRow,
            RenderedViewColumn,
            RenderedViewRow)
        {
            this.Vertical();
        }

        public void Next()
        {
            switch (this.current)
            {
                case View.Vertical:
                    this.Horizontal();
                    break;
                case View.Horizontal:
                    this.TextEditor();
                    break;
                case View.TextEditor:
                    this.RenderedView();
                    break;
                case View.RenderedView:
                    this.Vertical();
                    break;
            }
        }

        public void Previous()
        {
            switch (this.current)
            {
                case View.RenderedView:
                    this.TextEditor();
                    break;
                case View.TextEditor:
                    this.Horizontal();
                    break;
                case View.Horizontal:
                    this.Vertical();
                    break;
                case View.Vertical:
                    this.RenderedView();
                    break;
            }
        }

        private void Vertical()
        {
            this[LeftColumnWidth] = ON;
            this[RightColumnWidth] = ON;
            this[TopRowHeight] = ON;
            this[BottomRowHeight] = OFF;
            this[TextEditorColumn] = 0;
            this[TextEditorRow] = 0;
            this[RenderedViewColumn] = 1;
            this[RenderedViewRow] = 0;
            this.current = View.Vertical;
        }

        private void Horizontal()
        {
            this[LeftColumnWidth] = ON;
            this[RightColumnWidth] = OFF;
            this[TopRowHeight] = ON;
            this[BottomRowHeight] = ON;
            this[TextEditorColumn] = 0;
            this[TextEditorRow] = 0;
            this[RenderedViewColumn] = 0;
            this[RenderedViewRow] = 1;
            this.current = View.Horizontal;
        }

        private void TextEditor()
        {
            this[LeftColumnWidth] = ON;
            this[RightColumnWidth] = OFF;
            this[TopRowHeight] = ON;
            this[BottomRowHeight] = OFF;
            this[TextEditorColumn] = 0;
            this[TextEditorRow] = 0;
            this[RenderedViewColumn] = 1;
            this[RenderedViewRow] = 0;
            this.current = View.TextEditor;
        }

        private void RenderedView()
        {
            this[LeftColumnWidth] = OFF;
            this[RightColumnWidth] = ON;
            this[TopRowHeight] = ON;
            this[BottomRowHeight] = OFF;
            this[TextEditorColumn] = 0;
            this[TextEditorRow] = 0;
            this[RenderedViewColumn] = 1;
            this[RenderedViewRow] = 0;
            this.current = View.RenderedView;
        }
    }
}