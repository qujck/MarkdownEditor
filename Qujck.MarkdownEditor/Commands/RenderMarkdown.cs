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
    internal static partial class Command
    {
        internal static void Run(
            this ICommandRequestHandler<RenderMarkdown> handler,
            Action<string, object[]> callback,
            string markdown)
        {
            handler.Run(new RenderMarkdown(callback, markdown));
        }

        internal sealed class RenderMarkdown : ICommandRequest
        {
            internal RenderMarkdown(
                Action<string, object[]> callback, 
                string markdown)
            {
                this.Callback = callback;
                this.Markdown = markdown;
            }

            public Action<string, object[]> Callback { get; private set; }
            public string Markdown { get; private set; }
        }

        internal static partial class Handlers
        {
            internal sealed class RenderMarkdownHandler : ICommandRequestHandler<RenderMarkdown>
            {
                public void Run(RenderMarkdown command)
                {
                    command.Callback("renderMarkdown", new object[] { command.Markdown });
                }
            }
        }
    }
}
