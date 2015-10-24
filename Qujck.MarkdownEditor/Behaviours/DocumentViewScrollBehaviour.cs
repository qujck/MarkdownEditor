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
    internal sealed class DocumentViewScrollBehaviour : Behavior<DocumentView>
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
            this.AssociatedObject.TextEditor.TextArea.MouseWheel += TextArea_MouseWheel;
            this.AssociatedObject.RenderedView.LoadCompleted += RenderedView_LoadCompleted;
            var scroller = (ScrollViewer)VisualTreeHelper.GetChild(this.AssociatedObject.TextEditor, 0);
            scroller.ScrollChanged += Scroller_ScrollChanged;
        }

        private void AssociatedObject_Unloaded(object sender, RoutedEventArgs e)
        {
            this.AssociatedObject.TextEditor.TextArea.MouseWheel -= TextArea_MouseWheel;
            this.AssociatedObject.RenderedView.LoadCompleted -= RenderedView_LoadCompleted;
            var scroller = (ScrollViewer)VisualTreeHelper.GetChild(this.AssociatedObject.TextEditor, 0);
            scroller.ScrollChanged -= Scroller_ScrollChanged;
        }

        private void TextArea_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!e.Handled)
            {
                e.Handled = true;
                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                eventArg.RoutedEvent = UIElement.MouseWheelEvent;
                eventArg.Source = sender;
                var parent = ((Control)sender).Parent as UIElement;
                parent.RaiseEvent(eventArg);
            }
        }

        private void RenderedView_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        { 
            var document = (mshtml.IHTMLDocument2)this.AssociatedObject.RenderedView.Document;
            var window = (mshtml.IHTMLWindow3)document.parentWindow;
            window.attachEvent("onscroll", new ComEventListener(this.RenderedViewScrolled));
        }

        private void Scroller_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            this.AssociatedObject.TextEditor.ScrollToVerticalOffset(e.VerticalOffset);
            this.TextViewScrolled();
        }

        bool scrolling;
        private void RenderedViewScrolled()
        {
            if (!this.scrolling)
            {
                scrolling = true;
                this.SetTextEditorScrolledRatio(this.GetWebBrowserScrolledRatio());
                this.scrolling = false;
            }
        }

        private void TextViewScrolled()
        {
            if (!this.scrolling)
            {
                scrolling = true;
                this.SetWebBrowserScrolledRatio(this.GetTextEditorScrolledRatio());
                this.scrolling = false;
            }
        }

        private double GetTextEditorScrolledRatio()
        {
            return this.TextBoxScrollBarLength == 0
                ? 0
                : this.TextBoxScrollBarPosition / this.TextBoxScrollBarLength;
        }

        private void SetTextEditorScrolledRatio(double ratio)
        {
            this.AssociatedObject.TextEditor.ScrollToVerticalOffset(this.TextBoxScrollBarLength * ratio);
        }

        private double GetWebBrowserScrolledRatio()
        {
            return this.WebBrowserScrollBarLength == 0
                ? 0
                : this.WebBrowserScrollBarPosition / this.WebBrowserScrollBarLength;
        }

        private void SetWebBrowserScrolledRatio(double ratio)
        {
            if (this.HtmlTag != null)
            {
                this.WebBrowserScrollBarPosition = Convert.ToDouble(this.WebBrowserScrollBarLength) * ratio;
            }
        }

        private double TextBoxScrollBarPosition
        {
            get
            {
                return this.TextEditorVerticalScrollBar.Value;
            }
        }

        private double TextBoxScrollBarLength
        {
            get
            {
                // we need to adjust for the length of the "thumb" because the web browser version of this method works differently
                var bar = this.TextEditorVerticalScrollBar;
                var track = bar.Template.FindName("PART_Track", bar) as Track;

                if (track == null)
                {
                    return bar.Maximum;
                }
                else
                {
                    // http://stackoverflow.com/questions/3116287/setting-the-scrollbar-thumb-size
                    double thumbSize = (track.ViewportSize / (track.Maximum - track.Minimum + track.ViewportSize)) * track.ViewportSize;

                    double proportionOfScreenThatIsTheThumb = thumbSize / track.ViewportSize;

                    return bar.Maximum + (bar.Maximum * proportionOfScreenThatIsTheThumb);
                }
            }
        }

        private double WebBrowserScrollBarPosition
        {
            get
            {
                return Convert.ToDouble(this.HtmlTag.scrollTop);
            }
            set
            {
                this.HtmlTag.scrollTop = Convert.ToInt32(value);
            }
        }

        private double WebBrowserScrollBarLength
        {
            get
            {
                return Convert.ToDouble(this.HtmlTag.scrollHeight);
            }
        }

        private mshtml.IHTMLElement2 HtmlTag
        {
            get
            {
                var document = (mshtml.IHTMLDocument2)this.AssociatedObject.RenderedView.Document;
                var html = (mshtml.IHTMLElement2)document.all.tags("html")[0];

                return html;
            }
        }

        private ScrollBar TextEditorVerticalScrollBar
        {
            get
            {
                var scroller = (ScrollViewer)VisualTreeHelper.GetChild(this.AssociatedObject.TextEditor, 0);
                ScrollBar bar = scroller.Template.FindName("PART_VerticalScrollBar", scroller) as ScrollBar;

                return bar;
            }
        }
    }
}