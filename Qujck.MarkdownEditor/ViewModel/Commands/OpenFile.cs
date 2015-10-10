using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using Qujck.MarkdownEditor.Infrastructure;

namespace Qujck.MarkdownEditor.ViewModel.Commands
{
    internal sealed class OpenFile : ViewModelParameter
    {
        public OpenFile(DocumentViewModel viewModel) : base(viewModel)
        {
        }
    }

    internal sealed class OpenFileViewHandler : IViewModelCommand<OpenFile>
    {
        public void Execute(OpenFile viewModelParameter)
        {
            string text;
            string fileName = this.OpenFile(out text);
            if (fileName != null)
            {
                viewModelParameter.ViewModel[Constants.DocumentViewModel.FilePath] = fileName;
                viewModelParameter.ViewModel[Constants.DocumentViewModel.OpeningText] = text;
                viewModelParameter.ViewModel[Constants.DocumentViewModel.CurrentText] = text;
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
