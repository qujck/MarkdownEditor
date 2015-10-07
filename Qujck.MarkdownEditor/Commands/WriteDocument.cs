using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Qujck.MarkdownEditor.Infrastructure;
using Qujck.MarkdownEditor.Queries;

namespace Qujck.MarkdownEditor.Commands
{
    public static partial class Command
    {
        public static void Run(
            this ICommandHandler<WriteDocument> handler,
            Action<string, object[]> callback,
            string markdown)
        {
            handler.Run(new WriteDocument(callback, markdown));
        }

        public sealed class WriteDocument : ICommandParameter
        {
            internal WriteDocument(
                Action<string, object[]> callback, 
                string markdown)
            {
                this.Callback = callback;
                this.Markdown = markdown;
            }

            public Action<string, object[]> Callback { get; private set; }
            public string Markdown { get; private set; }
        }

        public static partial class Handlers
        {
            public sealed class WriteDocumentHandler : ICommandHandler<WriteDocument>
            {
                public void Run(WriteDocument command)
                {
                    command.Callback("renderMarkdown", new object[] { command.Markdown });
                }
            }
        }
    }
}
