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

namespace Qujck.MarkdownEditor
{
    public partial class MainForm : Form
    {
        private string pageLayout;

        public MainForm()
        {
            InitializeComponent();
            PreparePageLayout();
        }

        private void PreparePageLayout()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var html = assembly.GetManifestResourceStream("Qujck.MarkdownEditor.Content.layout.html");

            var scripts = new StringBuilder();
            var script = assembly.GetManifestResourceStream("Qujck.MarkdownEditor.Scripts.modernizr-2.8.3.js");
            scripts.AppendLine(new StreamReader(script).ReadToEnd());
            script = assembly.GetManifestResourceStream("Qujck.MarkdownEditor.Scripts.Prettify.prettify.js");
            scripts.AppendLine(new StreamReader(script).ReadToEnd());
            script = assembly.GetManifestResourceStream("Qujck.MarkdownEditor.Scripts.jquery-1.11.3.min.js");
            scripts.AppendLine(new StreamReader(script).ReadToEnd());
            script = assembly.GetManifestResourceStream("Qujck.MarkdownEditor.Scripts.bootstrap.min.js");
            scripts.AppendLine(new StreamReader(script).ReadToEnd());

            var langFiles =
                from name in assembly.GetManifestResourceNames()
                where name.StartsWith("Qujck.MarkdownEditor.Scripts.Prettify.lang-")
                let file = assembly.GetManifestResourceStream(name)
                select new StreamReader(file).ReadToEnd();
            langFiles.ToList().ForEach(langFile => scripts.AppendLine(langFile));

            var csss = new StringBuilder();
            var css = assembly.GetManifestResourceStream("Qujck.MarkdownEditor.Content.bootstrap.min.css");
            csss.AppendLine(new StreamReader(css).ReadToEnd());
            css = assembly.GetManifestResourceStream("Qujck.MarkdownEditor.Content.bootstrap-theme.min.css");
            csss.AppendLine(new StreamReader(css).ReadToEnd());
            css = assembly.GetManifestResourceStream("Qujck.MarkdownEditor.Content.site.css");
            csss.AppendLine(new StreamReader(css).ReadToEnd());
            css = assembly.GetManifestResourceStream("Qujck.MarkdownEditor.Content.stackoverflow.css");
            csss.AppendLine(new StreamReader(css).ReadToEnd());

            this.pageLayout = new StreamReader(html).ReadToEnd()
                .Replace("${style}", csss.ToString())
                .Replace("${script}", scripts.ToString());

            var md = assembly.GetManifestResourceStream("Qujck.MarkdownEditor.test.md");

            textView.Text = new StreamReader(md).ReadToEnd();
        }

        private void textView_TextChanged(object sender, EventArgs e)
        {
            var content = textView.Text.Replace(
                "![image](~",
                string.Format("![image]({0}", ""));

            var transformer = new Strike.IE.Markdownify();

            string markdown = transformer.Transform(content);

            markdown = markdown.Replace("<pre", "<pre class=\"prettyprint\"");

            renderedView.Url = new Uri("about:blank");
            var document = renderedView.Document.OpenNew(true);
            var text = this.pageLayout.Replace("${body}", markdown);
            document.Write(text);
            document.InvokeScript("prettyPrint");
        }
    }
}
