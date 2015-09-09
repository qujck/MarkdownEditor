using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Qujck.MarkdownEditor.Infrastructure;
using Qujck.MarkdownEditor.Queries;
using Qujck.MarkdownEditor.Commands;

namespace Qujck.MarkdownEditor
{
    public sealed partial class MainForm : Form, IDisposable
    {
        private readonly CompositionRoot resolver;

        public MainForm()
        {
            InitializeComponent();

            this.resolver = new CompositionRoot();
            this.InitialiseHtml();

            var md = this.resolver.Resolve<IStringResourceProvider>().Single("test.md");
            this.TextView.Text = md;
            this.RefreshTimer.Enabled = true;
        }

        private void InitialiseHtml()
        {
            var htmlQuery = this.resolver.Resolve<IQueryHandler<Query.Html, string>>();

            string html = htmlQuery.Execute();
            this.RenderedView.Load(html);
        }

        private void TextView_TextChanged(object sender, EventArgs e)
        {
            if (!this.RefreshTimer.Enabled)
            {
                this.RefreshTimer.Enabled = true;
            }
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            this.RefreshView();
        }

        private void RefreshView()
        {
            if (RefreshTimer.Enabled && this.RefreshTimer.Tag == null)
            {
                RefreshTimer.Enabled = false;
                this.RefreshTimer.Tag = this;

                var writeCommand = this.resolver.Resolve<ICommandHandler<Command.WriteDocument>>();

                writeCommand.Run(
                    this.RenderedView.Document,
                    TextView.Text);

                this.RefreshTimer.Tag = null;
            }
        }
    }
}
