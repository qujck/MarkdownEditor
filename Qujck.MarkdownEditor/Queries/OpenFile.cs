using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using Qujck.MarkdownEditor.Infrastructure;

namespace Qujck.MarkdownEditor.Queries
{
    public static partial class Query
    {
        public static string Execute(this IQueryHandler<OpenFile, string> handler)
        {
            return handler.Execute(new OpenFile());
        }

        public sealed class OpenFile : IQueryParameter<string>
        {
            internal OpenFile() { }
        }

        public static partial class Handlers
        {
            public sealed class OpenFileHandler : IQueryHandler<OpenFile, string>
            {
                public string Execute(OpenFile query)
                {
                    var dialog = new OpenFileDialog()
                    {
                        Filter = "Markdown Files(*.md)|*.md|All(*.*)|*"
                    };

                    return dialog.ShowDialog() == true
                        ? File.ReadAllText(dialog.FileName)
                        : null;
                }
            }
        }
    }
}
