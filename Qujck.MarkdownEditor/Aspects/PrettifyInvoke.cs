using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qujck.MarkdownEditor.Infrastructure;
using Qujck.MarkdownEditor.Commands;

namespace Qujck.MarkdownEditor.Aspects
{
    public sealed class PrettifyInvoke : ICommandHandler<Command.WriteDocument>
    {
        private readonly ICommandHandler<Command.WriteDocument> decorated;

        public PrettifyInvoke(ICommandHandler<Command.WriteDocument> decorated)
        {
            this.decorated = decorated;
        }

        public void Run(Command.WriteDocument command)
        {
            this.decorated.Run(command);

            command.WebBrowser.Document.InvokeScript("prettyPrint");
        }
    }
}
