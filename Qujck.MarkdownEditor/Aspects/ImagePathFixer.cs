using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Qujck.MarkdownEditor.Infrastructure;
using Qujck.MarkdownEditor.Actions;

namespace Qujck.MarkdownEditor.Aspects
{
    internal sealed class ImagePathFixer : IActionRequestHandler<Command.RenderMarkdown>
    {
        private readonly IActionRequestHandler<Command.RenderMarkdown> decorated;

        public ImagePathFixer(IActionRequestHandler<Command.RenderMarkdown> decorated)
        {
            this.decorated = decorated;
        }

        public void Run(Command.RenderMarkdown command)
        {
            string text = command.Markdown.Replace(
                "![image](~",
                string.Format("![image]({0}", System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\" ));

            this.decorated.Run(command.Action, text);
        }
    }
}
