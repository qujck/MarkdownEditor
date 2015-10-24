﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qujck.MarkdownEditor.Infrastructure;
using Qujck.MarkdownEditor.Actions;

namespace Qujck.MarkdownEditor.Aspects
{
    internal sealed class PrettifyInvoke : IActionRequestHandler<Command.RenderMarkdown>
    {
        private readonly IActionRequestHandler<Command.RenderMarkdown> decorated;

        public PrettifyInvoke(IActionRequestHandler<Command.RenderMarkdown> decorated)
        {
            this.decorated = decorated;
        }

        public void Run(Command.RenderMarkdown command)
        {
            this.decorated.Run(command);

            command.Action("prettifyCodeSamples", null);
        }
    }
}
