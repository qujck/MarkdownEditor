using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Qujck.MarkdownEditor.Commands;
using Qujck.MarkdownEditor.Infrastructure;
using Qujck.MarkdownEditor.Queries;

namespace Qujck.MarkdownEditor
{
    public sealed partial class DocumentViewModel : DynamicViewModel
    {
        public ICommandHandler<Command.SaveFile> SaveFileHandler { private get; set; }

        public IQueryHandler<Query.OpenFile, string> OpenFileHandler { private get; set; }

        public DocumentViewModel() : base(VerticalView, Data)
        {
            base.RegisterCommandAction(
                "_Next", 
                () => this.Update(WhatsNext((View)this[CurrentView])));
            base.RegisterCommandAction(
                "_Previous", 
                () => this.Update(WhatsPrior((View)this[CurrentView])));
            base.RegisterCommandAction(
                "_Save",
                () => this.Save(),
                "_CanSave",
                () => (string)this[OpeningText] != (string)this[CurrentText]);
            base.RegisterCommandAction(
                "_Open",
                () => Open());
        }

        public void Update(string text)
        {
            this[CurrentText] = text;
        }

        public void Open()
        {
            var text = this.OpenFileHandler.Execute();
            if (text != null)
            {
                this[OpeningText] = text;
                this[CurrentText] = text;
            }
        }

        public void Open(string text)
        {
            this[OpeningText] = text;
            this[CurrentText] = text;
        }

        private void Save()
        {
            this.SaveFileHandler.Run((string)this[CurrentText]);
            this[OpeningText] = this[CurrentText];
        }

        private readonly static IDictionary<string, object> Data =
            new Dictionary<string, object>
            {
                { OpeningText, null },
                { CurrentText, null }
            };

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

        private static IDictionary<string, object> WhatsPrior(View currentView)
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