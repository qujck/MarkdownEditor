using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qujck.MarkdownEditor.Infrastructure;
using Qujck.MarkdownEditor.Commands;

namespace Qujck.MarkdownEditor.Aspects
{
    public sealed class PrettifyInvoke : ICommandHandler<Command.RenderMarkdown>
    {
        private readonly ICommandHandler<Command.RenderMarkdown> decorated;

        public PrettifyInvoke(ICommandHandler<Command.RenderMarkdown> decorated)
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
