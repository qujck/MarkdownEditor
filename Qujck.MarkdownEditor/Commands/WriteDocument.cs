using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Qujck.MarkdownEditor.Queries;

namespace Qujck.MarkdownEditor.Commands
{
    public static partial class Command
    {
        public static void Run(
            this ICommandHandler<WriteDocument> handler,
            WebBrowser webBrowser,
            string markdown)
        {
            handler.Run(new WriteDocument(webBrowser, markdown));
        }

        public sealed class WriteDocument : ICommand
        {
            internal WriteDocument(
                WebBrowser webBrowser, 
                string markdown)
            {
                this.WebBrowser = webBrowser;
                this.Markdown = markdown;
            }

            public WebBrowser WebBrowser { get; private set; }
            public string Markdown { get; private set; }
        }

        internal static partial class Handlers
        {
            public sealed class WriteDocumentHandler : ICommandHandler<WriteDocument>
            {
                private readonly IQueryHandler<Query.Html, string> html;

                public WriteDocumentHandler(IQueryHandler<Query.Html, string> html)
                {
                    this.html = html;
                }

                public void Run(WriteDocument command)
                {
                    command.WebBrowser.Url = new Uri("about:blank");
                    var document = command.WebBrowser.Document.OpenNew(true);
                    var text = this.html.Execute().Replace("${body}", command.Markdown);
                    document.Write(text);
                }
            }
        }
    }
}
