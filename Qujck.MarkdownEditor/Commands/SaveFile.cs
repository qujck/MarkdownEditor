using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using Qujck.MarkdownEditor.Infrastructure;
using Qujck.MarkdownEditor.Queries;

namespace Qujck.MarkdownEditor.Commands
{
    public static partial class Command
    {
        public static void Run(
            this ICommandHandler<SaveFile> handler,
            string markdown)
        {
            handler.Run(new SaveFile(markdown));
        }

        public sealed class SaveFile : ICommandParameter
        {
            internal SaveFile(
                string markdown)
            {
                this.Markdown = markdown;
            }

            public string Markdown { get; private set; }
        }

        public static partial class Handlers
        {
            public sealed class SaveFileHandler : ICommandHandler<SaveFile>
            {
                public void Run(SaveFile command)
                {
                    var dialog = new SaveFileDialog()
                    {
                        Filter = "Markdown Files(*.md)|*.md|All(*.*)|*"
                    };

                    if (dialog.ShowDialog() == true)
                    {
                        File.WriteAllText(dialog.FileName, command.Markdown);
                    }
                }
            }
        }
    }
}
