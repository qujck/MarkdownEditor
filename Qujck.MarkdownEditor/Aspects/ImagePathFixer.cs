using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Qujck.MarkdownEditor.Infrastructure;
using Qujck.MarkdownEditor.Requests;

namespace Qujck.MarkdownEditor.Aspects
{
    internal sealed class ImagePathFixer : IActionRequestHandler<Actions.RenderMarkdown>
    {
        private readonly IActionRequestHandler<Actions.RenderMarkdown> decorated;

        public ImagePathFixer(IActionRequestHandler<Actions.RenderMarkdown> decorated)
        {
            this.decorated = decorated;
        }

        public void Run(Actions.RenderMarkdown command)
        {
            string text = command.Markdown.Replace(
                "![image](~",
                string.Format("![image]({0}", System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\" ));

            this.decorated.Run(command.Action, text);
        }
    }
}
