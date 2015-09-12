using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
            InitializeComponent();

            this.textChangedRefreshRenderedViewTimer = new DispatcherTimer { Interval = new TimeSpan(100) };
            this.textChangedRefreshRenderedViewTimer.Tick += RefreshTimer_Tick;
            this.textChangedRefreshRenderedViewTimer.Start();

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
                    window.attachEvent("onscroll", new EventListener(this.Scrolled));
                };

            this.resolver = new CompositionRoot();

            this.InitialiseHtml();

            var md = this.resolver.Resolve<IStringResourceProvider>().Single("test.md");
            this.TextEditor.Text = md;
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
            if (this.textChangedRefreshRenderedViewTimer.IsEnabled && this.textChangedRefreshRenderedViewTimer.Tag == null)
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

        private int top = 0;
        private void Scrolled()
        {
            var document = (mshtml.IHTMLDocument2)this.RenderedView.Document;
            var element = (mshtml.IHTMLElement2)document.all.tags("html")[0];
            int latest = Convert.ToInt32(element.scrollTop);
            if (latest != top)
            {
                top = latest;
                Debug.WriteLine(top);
            }
        }

        [ComVisible(true)]
        [ClassInterface(ClassInterfaceType.AutoDispatch)]
        public class EventListener
        {
            private readonly Action action;

            public EventListener(Action action)
            {
                this.action = action;
            }

            [DispId(0)]
            public void NameDoesNotMatter(object data)
            {
                this.action();
            }
        }

        private void ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            this.TextEditor.ScrollToVerticalOffset(e.VerticalOffset);
            Debug.WriteLine(e.VerticalOffset);
        }

    }
}
