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

            var md = ResourceHelpers.ReadResource("Qujck.MarkdownEditor.test.md");
            TextView.Text = md;
            TextView.Select(0, 0);
        }

        private void InitialiseHtml()
        {
            var htmlQuery = this.resolver.Resolve<IQueryHandler<Query.Html, string>>();

            string html = htmlQuery.Execute();
            this.RenderedView.Url = new Uri("about:blank");
            var document = RenderedView.Document;
            document.Write(html);
        }

        private void TextView_TextChanged(object sender, EventArgs e)
        {
            var writeCommand = this.resolver.Resolve<ICommandHandler<Command.WriteDocument>>();

            writeCommand.Run(
                this.RenderedView.Document, 
                TextView.Text);
        }
    }
}
