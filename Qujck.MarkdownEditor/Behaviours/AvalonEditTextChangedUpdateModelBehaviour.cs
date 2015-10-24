﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;
using Qujck.MarkdownEditor.ViewModel;

namespace Qujck.MarkdownEditor.Behaviours
{
    internal sealed class AvalonEditTextChangedUpdateModelBehaviour : Behavior<DocumentView>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            base.AssociatedObject.Loaded += AssociatedObject_Loaded;
            base.AssociatedObject.Unloaded += AssociatedObject_Unloaded;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            base.AssociatedObject.Loaded -= AssociatedObject_Loaded;
            base.AssociatedObject.Unloaded -= AssociatedObject_Unloaded;
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            this.AssociatedObject.TextEditor.TextChanged += TextEditor_TextChanged;
        }

        private void AssociatedObject_Unloaded(object sender, RoutedEventArgs e)
        {
            this.AssociatedObject.TextEditor.TextChanged -= TextEditor_TextChanged;
        }

        private void TextEditor_TextChanged(object sender, EventArgs e)
        {
            var viewModel = this.AssociatedObject.DataContext as DocumentViewModel;
            viewModel[Constants.DocumentViewModel.CurrentText] = this.AssociatedObject.TextEditor.Text;
        }
    }
}
