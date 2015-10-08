using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Threading;
using ICSharpCode.AvalonEdit;
using Qujck.MarkdownEditor.Queries;
using Qujck.MarkdownEditor.Commands;
using Qujck.MarkdownEditor.Infrastructure;

namespace Qujck.MarkdownEditor.Behaviours
{
    public sealed class DocumentViewFocusBehaviour : Behavior<DocumentView>
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
            this.AssociatedObject.RenderedView.LoadCompleted += RenderedView_LoadCompleted;
        }

        private void AssociatedObject_Unloaded(object sender, RoutedEventArgs e)
        {
            this.AssociatedObject.RenderedView.LoadCompleted -= RenderedView_LoadCompleted;
        }

        private void RenderedView_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            var document = (mshtml.IHTMLDocument2)this.AssociatedObject.RenderedView.Document;
            var window = (mshtml.IHTMLWindow3)document.parentWindow;
            window.attachEvent("onfocus", new ComEventListener(this.TransferFocus));
            this.TransferFocus();
        }

        private void TransferFocus()
        {
            this.AssociatedObject.TextEditor.Focus();
        }
   }
}