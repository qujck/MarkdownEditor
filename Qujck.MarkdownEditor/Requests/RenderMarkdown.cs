using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Qujck.MarkdownEditor.Infrastructure;

namespace Qujck.MarkdownEditor.Requests
{
    internal static partial class Actions
    {
        internal static void Run(
            this IActionRequestHandler<RenderMarkdown> handler,
            Action<string, object[]> action,
            string markdown)
        {
            handler.Run(new RenderMarkdown(action, markdown));
        }

        internal sealed class RenderMarkdown : IActionRequest
        {
            internal RenderMarkdown(
                Action<string, object[]> action, 
                string markdown)
            {
                this.Action = action;
                this.Markdown = markdown;
            }

            public Action<string, object[]> Action { get; private set; }
            public string Markdown { get; private set; }
        }

        internal static partial class Handlers
        {
            internal sealed class RenderMarkdownHandler : IActionRequestHandler<RenderMarkdown>
            {
                public void Run(RenderMarkdown command)
                {
                    command.Action("renderMarkdown", new object[] { command.Markdown });
                }
            }
        }
    }
}
