using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qujck.MarkdownEditor.Infrastructure;
using Qujck.MarkdownEditor.Commands;

namespace Qujck.MarkdownEditor.Aspects
{
    public sealed class PrettifyInvoke : ICommandService<Command.WriteDocument>
    {
        private readonly ICommandService<Command.WriteDocument> decorated;

        public PrettifyInvoke(ICommandService<Command.WriteDocument> decorated)
        {
            this.decorated = decorated;
        }

        public void Run(Command.WriteDocument command)
        {
            this.decorated.Run(command);

            command.Callback("prettifyCodeSamples", null);
        }
    }
}
