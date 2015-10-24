﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Qujck.MarkdownEditor.Infrastructure;
using Qujck.MarkdownEditor.Commands;

namespace Qujck.MarkdownEditor.Aspects
{
    internal sealed class ImagePathFixer : ICommandRequestHandler<Command.RenderMarkdown>
    {
        private readonly ICommandRequestHandler<Command.RenderMarkdown> decorated;

        public ImagePathFixer(ICommandRequestHandler<Command.RenderMarkdown> decorated)
        {
            this.decorated = decorated;
        }

        public void Run(Command.RenderMarkdown command)
        {
            string text = command.Markdown.Replace(
                "![image](~",
                string.Format("![image]({0}", System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\" ));

            this.decorated.Run(command.Callback, text);
        }
    }
}
