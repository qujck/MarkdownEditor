using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qujck.MarkdownEditor.Infrastructure;
using Qujck.MarkdownEditor.Requests;

namespace Qujck.MarkdownEditor.Aspects
{
    internal sealed class PrettifyInvoke : IActionRequestHandler<Actions.RenderMarkdown>
    {
        private readonly IActionRequestHandler<Actions.RenderMarkdown> decorated;

        public PrettifyInvoke(IActionRequestHandler<Actions.RenderMarkdown> decorated)
        {
            this.decorated = decorated;
        }

        public void Run(Actions.RenderMarkdown command)
        {
            this.decorated.Run(command);

            command.Action("prettifyCodeSamples", null);
        }
    }
}
