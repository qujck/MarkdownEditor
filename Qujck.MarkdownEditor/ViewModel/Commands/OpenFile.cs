using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using Qujck.MarkdownEditor.Requests;
using Qujck.MarkdownEditor.ViewModel.Core;
using System.Windows;

namespace Qujck.MarkdownEditor.ViewModel.Commands
{
    internal sealed class OpenFile : DynamicViewModelParameter
    {
        public OpenFile(DocumentViewModel viewModel) : base(viewModel)
        {
        }
    }

    internal sealed class OpenFileHandler : IViewModelCommand<OpenFile>
    {
        private readonly IActionRequestHandler<Actions.LoadWholeFile> loader;

        public OpenFileHandler(IActionRequestHandler<Actions.LoadWholeFile> loader)
        {
            this.loader = loader;
        }

        public void Run(OpenFile viewModelParameter)
        {
            string fileName = this.GetFileName();
            if (fileName != null)
            {
                this.loader.Run(viewModelParameter.DynamicViewModel as DocumentViewModel, fileName);
            }
        }

        public string GetFileName()
        {
            var dialog = new OpenFileDialog()
            {
                Filter = "Markdown Files(*.md)|*.md|All(*.*)|*"
            };

            if (dialog.ShowDialog() == true)
            {
                return dialog.FileName;
            }
            else
            {
                return null;
            }
        }
    }
}
