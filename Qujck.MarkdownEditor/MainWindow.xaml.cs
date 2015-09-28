using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using ICSharpCode.AvalonEdit;
using Qujck.MarkdownEditor.Infrastructure;
using Qujck.MarkdownEditor.Queries;
using Qujck.MarkdownEditor.Commands;

namespace Qujck.MarkdownEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly CompositionRoot resolver;
        private readonly DispatcherTimer textChangedRefreshRenderedViewTimer;

        public MainWindow()
        {
            this.resolver = new CompositionRoot();

            InitializeComponent();

            this.textChangedRefreshRenderedViewTimer = new DispatcherTimer { Interval = new TimeSpan(100) };
            this.textChangedRefreshRenderedViewTimer.Tick += RefreshTimer_Tick;
            //this.textChangedRefreshRenderedViewTimer.Start();

            this.TextEditor.TextArea.MouseWheel += (sender, e) =>
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
                };

            this.TextEditor.TextChanged += (sender, e) =>
                {
                    if (!this.textChangedRefreshRenderedViewTimer.IsEnabled)
                    {
                        this.textChangedRefreshRenderedViewTimer.Start();
                    }
                };

            this.RenderedView.LoadCompleted += (sender, e) =>
                {
                    var document = (mshtml.IHTMLDocument2)this.RenderedView.Document;
                    var window = (mshtml.IHTMLWindow3)document.parentWindow;
                    window.attachEvent("onscroll", new ComEventListener(this.RenderedViewScrolled));

                    var md = this.resolver.Resolve<IStringResourceProvider>().One("test.md");
                    this.TextEditor.Text = md;
                };

            this.InitialiseHtml();
        }

        private void InitialiseHtml()
        {
            var htmlQuery = this.resolver.Resolve<IQueryHandler<Query.Html, string>>();

            string html = htmlQuery.Execute();
            this.RenderedView.NavigateToString(html);
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            this.RefreshView();
        }

        private void RefreshView()
        {
            if (this.textChangedRefreshRenderedViewTimer.IsEnabled && 
                this.textChangedRefreshRenderedViewTimer.Tag == null)
            {
                this.textChangedRefreshRenderedViewTimer.Stop();
                this.textChangedRefreshRenderedViewTimer.Tag = this;

                var writeCommand = this.resolver.Resolve<ICommandHandler<Command.WriteDocument>>();

                writeCommand.Run(
                    (scriptName, args) => this.RenderedView.InvokeScript(scriptName, args),
                    this.TextEditor.Text);

                this.textChangedRefreshRenderedViewTimer.Tag = null;
            }
        }

        private void TextView_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            this.TextEditor.ScrollToVerticalOffset(e.VerticalOffset);
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
            this.TextEditor.ScrollToVerticalOffset(this.TextBoxScrollBarLength * ratio);
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
                 return  Convert.ToDouble(this.HtmlTag.scrollHeight);
            }
        }

        private mshtml.IHTMLElement2 HtmlTag
        {
            get
            {
                var document = (mshtml.IHTMLDocument2)this.RenderedView.Document;
                var html = (mshtml.IHTMLElement2)document.all.tags("html")[0];

                return html;
            }
        }

        private ScrollBar TextEditorVerticalScrollBar
        {
            get
            {
                var scroller = (ScrollViewer)VisualTreeHelper.GetChild(this.TextEditor, 0);
                ScrollBar bar = scroller.Template.FindName("PART_VerticalScrollBar", scroller) as ScrollBar;

                return bar;
            }
        }
    }
}
