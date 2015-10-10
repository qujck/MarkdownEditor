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

namespace Qujck.MarkdownEditor.ViewModel
{
    public sealed partial class DocumentViewModel : DynamicViewModel
    {
        public ICommandHandler<Command.SaveFile> SaveFileHandler { private get; set; }

        public IQueryHandler<Query.OpenFile, string> OpenFileHandler { private get; set; }

        public DocumentViewModel() : base(Constants.DocumentViewModel.Views[0], Data)
        {
            base.RegisterCommandAction(
                "_Next", 
                () => {
                    int next = WhatsNext(this[Constants.DocumentViewModel.CurrentView]);
                    this[Constants.DocumentViewModel.CurrentView] = next;
                    this.Update(Constants.DocumentViewModel.Views[next]);
                });
            base.RegisterCommandAction(
                "_Previous",
                () => {
                    int prior = WhatsPrior(this[Constants.DocumentViewModel.CurrentView]);
                    this[Constants.DocumentViewModel.CurrentView] = prior;
                    this.Update(Constants.DocumentViewModel.Views[prior]);
                });
            base.RegisterCommandAction(
                "_Save",
                () => this.Save(),
                "_CanSave",
                () => (string)this[Constants.DocumentViewModel.OpeningText] != 
                    (string)this[Constants.DocumentViewModel.CurrentText]);
            base.RegisterCommandAction(
                "_Open",
                () => Open());
        }

        public void Update(string text)
        {
            this[Constants.DocumentViewModel.CurrentText] = text;
        }

        public void Open()
        {
            var text = this.OpenFileHandler.Execute();
            if (text != null)
            {
                this[Constants.DocumentViewModel.OpeningText] = text;
                this[Constants.DocumentViewModel.CurrentText] = text;
            }
        }

        public void Open(string text)
        {
            this[Constants.DocumentViewModel.OpeningText] = text;
            this[Constants.DocumentViewModel.CurrentText] = text;
        }

        private void Save()
        {
            this.SaveFileHandler.Run((string)this[Constants.DocumentViewModel.CurrentText]);
            this[Constants.DocumentViewModel.OpeningText] = this[Constants.DocumentViewModel.CurrentText];
        }

        private readonly static IDictionary<string, object> Data =
            new Dictionary<string, object>
            {
                { Constants.DocumentViewModel.OpeningText, null },
                { Constants.DocumentViewModel.CurrentText, null },
                { Constants.DocumentViewModel.CurrentView, 0 }
            };

        private static int WhatsNext(object currentView)
        {
            int next = (int)currentView + 1;
            return Constants.DocumentViewModel.Views.Length == next
                ? 0
                : next;
        }

        private static int WhatsPrior(object currentView)
        {
            int prior = (int)currentView - 1;
            return prior < 0
                ? 0
                : Constants.DocumentViewModel.Views.Length - 1;
        }
    }
}