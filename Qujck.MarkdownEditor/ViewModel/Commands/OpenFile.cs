using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using Qujck.MarkdownEditor.ViewModel.Core;

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
        public void Run(OpenFile viewModelParameter)
        {
            string text;
            string fileName = this.OpenFile(out text);
            if (fileName != null)
            {
                viewModelParameter[Constants.DocumentViewModel.FilePath] = fileName;
                viewModelParameter[Constants.DocumentViewModel.OpeningText] = text;
                viewModelParameter[Constants.DocumentViewModel.CurrentText] = text;
            }
        }

        public string OpenFile(out string text)
        {
            var dialog = new OpenFileDialog()
            {
                Filter = "Markdown Files(*.md)|*.md|All(*.*)|*"
            };

            if (dialog.ShowDialog() == true)
            {
                text = File.ReadAllText(dialog.FileName);
                return dialog.FileName;
            }
            else
            {
                text = null;
                return null;
            }
        }
    }
}
