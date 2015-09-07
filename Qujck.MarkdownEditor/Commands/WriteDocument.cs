using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Qujck.MarkdownEditor.Infrastructure;
using Qujck.MarkdownEditor.Queries;

namespace Qujck.MarkdownEditor.Commands
{
    public static partial class Command
    {
        public static void Run(
            this ICommandHandler<WriteDocument> handler,
            HtmlDocument document,
            string markdown)
        {
            handler.Run(new WriteDocument(document, markdown));
        }

        public sealed class WriteDocument : ICommand
        {
            internal WriteDocument(
                HtmlDocument document, 
                string markdown)
            {
                this.Document = document;
                this.Markdown = markdown;
            }

            public HtmlDocument Document { get; private set; }
            public string Markdown { get; private set; }
        }

        public static partial class Handlers
        {
            public sealed class WriteDocumentHandler : ICommandHandler<WriteDocument>
            {
                public void Run(WriteDocument command)
                {
                    command.Document.InvokeScript("renderMarkdown", new object[] { command.Markdown });
                }
            }
        }
    }
}
