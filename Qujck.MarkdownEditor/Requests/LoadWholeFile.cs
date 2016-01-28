using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Qujck.MarkdownEditor.ViewModel;

namespace Qujck.MarkdownEditor.Requests
{
    internal static partial class Actions
    {
        internal static void Run(
            this IActionRequestHandler<LoadWholeFile> handler,
            DocumentViewModel viewModel,
            string name)
        {
            handler.Run(new LoadWholeFile(viewModel, name));
        }

        internal sealed class LoadWholeFile : IActionRequest
        {
            internal LoadWholeFile(
                DocumentViewModel viewModel,
                string name)
            {
                this.ViewModel = viewModel;
                this.Name = name;
            }

            public DocumentViewModel ViewModel { get; set; }
            public string Name { get; set; }

        }

        internal static partial class Handlers
        {
            internal sealed class LoadWholeFileHandler : IActionRequestHandler<LoadWholeFile>
            {
                public void Run(LoadWholeFile query)
                {
                    var text = File.ReadAllText(query.Name);
                    query.ViewModel[Constants.DocumentViewModel.FilePath] = query.Name;
                    query.ViewModel[Constants.DocumentViewModel.OpeningText] = text;
                    query.ViewModel[Constants.DocumentViewModel.CurrentText] = text;
                }
            }
        }
    }
}
