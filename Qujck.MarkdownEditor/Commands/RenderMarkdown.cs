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
            this ICommandHandler<RenderMarkdown> handler,
            Action<string, object[]> callback,
            string markdown)
        {
            handler.Run(new RenderMarkdown(callback, markdown));
        }

        public sealed class RenderMarkdown : ICommandParameter
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

        public static partial class Handlers
        {
            public sealed class RenderMarkdownHandler : ICommandHandler<RenderMarkdown>
            {
                public void Run(RenderMarkdown command)
                {
                    command.Callback("renderMarkdown", new object[] { command.Markdown });
                }
            }
        }
    }
}
