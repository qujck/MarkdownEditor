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
using Qujck.MarkdownEditor.ViewModel;

namespace Qujck.MarkdownEditor.Behaviours
{
    public sealed class DocumentViewInitialiseRenderedView : Behavior<DocumentView>
    {
        public IQueryHandler<Query.Html, string> HtmlQueryHandler { private get; set; }

        protected override void OnAttached()
        {
            base.OnAttached();
            base.AssociatedObject.Loaded += AssociatedObject_Loaded;
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            string html = this.HtmlQueryHandler.Execute();
            this.AssociatedObject.RenderedView.LoadCompleted += RenderedView_LoadCompleted;
            this.AssociatedObject.RenderedView.NavigateToString(html);
        }

        private void RenderedView_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            this.HtmlIsLoaded = true;
        }

        private bool HtmlIsLoaded
        {
            set
            {
                var model = (DocumentViewModel)this.AssociatedObject.DataContext;
                model.HtmlIsLoaded = value;
            }
        }
    }
}
