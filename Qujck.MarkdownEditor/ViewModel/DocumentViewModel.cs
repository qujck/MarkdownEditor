﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Qujck.MarkdownEditor.Requests;
using Qujck.MarkdownEditor.Infrastructure;

namespace Qujck.MarkdownEditor.ViewModel
{
    public sealed partial class DocumentViewModel : DynamicViewModel
    {
        public DocumentViewModel() : base(Constants.DocumentViewModel.Views[0], Data)
        {
        }

        private readonly static IDictionary<string, object> Data =
            new Dictionary<string, object>
            {
                { Constants.DocumentViewModel.OpeningText, null },
                { Constants.DocumentViewModel.CurrentText, null },
                { Constants.DocumentViewModel.CurrentView, 0 },
                { Constants.DocumentViewModel.FilePath, null },
                { Constants.DocumentViewModel.HtmlIsLoaded, null }
            };

        public bool HtmlIsLoaded
        {
            get
            {
                bool result = this[Constants.DocumentViewModel.HtmlIsLoaded] == null
                    ? false
                    : (bool)this[Constants.DocumentViewModel.HtmlIsLoaded];
                return result;
            }
            set
            {
                this[Constants.DocumentViewModel.HtmlIsLoaded] = value;
            }
        }

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