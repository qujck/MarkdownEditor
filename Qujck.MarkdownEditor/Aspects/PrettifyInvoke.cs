using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qujck.MarkdownEditor.Infrastructure;
using Qujck.MarkdownEditor.Commands;

namespace Qujck.MarkdownEditor.Aspects
{
    internal sealed class PrettifyInvoke : ICommandRequestHandler<Command.RenderMarkdown>
    {
        private readonly ICommandRequestHandler<Command.RenderMarkdown> decorated;

        public PrettifyInvoke(ICommandRequestHandler<Command.RenderMarkdown> decorated)
        {
            this.decorated = decorated;
        }

        public void Run(Command.RenderMarkdown command)
        {
            this.decorated.Run(command);

            command.Callback("prettifyCodeSamples", null);
        }
    }
}
