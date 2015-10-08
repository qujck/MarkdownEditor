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
    public sealed partial class DocumentViewModel : AbstractViewModel
    {
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
    }
}