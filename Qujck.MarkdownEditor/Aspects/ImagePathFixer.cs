using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Qujck.MarkdownEditor.Infrastructure;
using Qujck.MarkdownEditor.Commands;

namespace Qujck.MarkdownEditor.Aspects
{
    public sealed class ImagePathFixer : ICommandService<Command.WriteDocument>
    {
        private readonly ICommandService<Command.WriteDocument> decorated;

        public ImagePathFixer(ICommandService<Command.WriteDocument> decorated)
        {
            this.decorated = decorated;
        }

        public void Run(Command.WriteDocument command)
        {
            string text = command.Markdown.Replace(
                "![image](~",
                string.Format("![image]({0}", System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\" ));

            this.decorated.Run(command.Callback, text);
        }
    }
}
