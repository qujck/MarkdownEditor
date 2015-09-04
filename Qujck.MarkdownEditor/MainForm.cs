using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Qujck.MarkdownEditor.Queries;

namespace Qujck.MarkdownEditor
{
    public partial class MainForm : Form
    {
        private CompositionRoot resolver;
        private string pageLayout;

        public MainForm()
        {
            InitializeComponent();
            this.resolver = new CompositionRoot();
            PreparePageLayout();

            var md = ResourceHelpers.ReadResource("Qujck.MarkdownEditor.test.md");
            textView.Text = md;
            textView.Select(0, 0);
        }

        private void PreparePageLayout()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var html = this.resolver.Resolve<IQueryHandler<Query.Html, string>>()
                .Execute();
            var styles = this.resolver.Resolve<IQueryHandler<Query.Styles, string>>()
                .Execute();
            var scripts = this.resolver.Resolve<IQueryHandler<Query.Scripts, string>>()
                .Execute();

            this.pageLayout = html
                .Replace("${style}", styles)
                .Replace("${script}", scripts);
        }

        private void textView_TextChanged(object sender, EventArgs e)
        {
            var markdown = this.resolver.Resolve<IQueryHandler<Query.Markdown, string>>()
                .Execute(textView.Text);

            renderedView.Url = new Uri("about:blank");
            var document = renderedView.Document.OpenNew(true);
            var text = this.pageLayout.Replace("${body}", markdown);
            document.Write(text);
            document.InvokeScript("prettyPrint");
        }
    }
}
