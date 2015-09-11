using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        private readonly DispatcherTimer refreshTimer;
        private readonly DispatcherTimer scrollTimer;

        public MainWindow()
        {
            InitializeComponent();

            this.resolver = new CompositionRoot();

            this.InitialiseHtml();

            var md = this.resolver.Resolve<IStringResourceProvider>().Single("test.md");
            this.TextEditor.Text = md;

            this.refreshTimer = new DispatcherTimer { Interval = new TimeSpan(100) };
            this.refreshTimer.Tick += RefreshTimer_Tick;
            this.refreshTimer.Start();

            this.scrollTimer = new DispatcherTimer { Interval = new TimeSpan(100) };
            this.scrollTimer.Tick += ScrollTimer_Tick;
            this.scrollTimer.Start();
        }

        private void InitialiseHtml()
        {
            var htmlQuery = this.resolver.Resolve<IQueryHandler<Query.Html, string>>();

            string html = htmlQuery.Execute();
            this.RenderedView.NavigateToString(html);
        }

        private void TextView_TextChanged(object sender, EventArgs e)
        {
            if (!this.refreshTimer.IsEnabled)
            {
                this.refreshTimer.Start();
            }
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            this.RefreshView();
        }

        private void RefreshView()
        {
            if (this.refreshTimer.IsEnabled && this.refreshTimer.Tag == null)
            {
                this.refreshTimer.Stop();
                this.refreshTimer.Tag = this;

                var writeCommand = this.resolver.Resolve<ICommandHandler<Command.WriteDocument>>();

                writeCommand.Run(
                    (scriptName, args) => this.RenderedView.InvokeScript(scriptName, args),
                    this.TextEditor.Text);

                this.refreshTimer.Tag = null;
            }
        }

        private int top = 0;
        private void ScrollTimer_Tick(object sender, EventArgs e)
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

        private void ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            this.TextEditor.ScrollToVerticalOffset(e.VerticalOffset);
            Debug.WriteLine(e.VerticalOffset);
        }
    }
}
