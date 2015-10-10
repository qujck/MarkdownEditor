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
            var text = this.OpenFile();
            if (text != null)
            {
                viewModelParameter.ViewModel[Constants.DocumentViewModel.OpeningText] = text;
                viewModelParameter.ViewModel[Constants.DocumentViewModel.CurrentText] = text;
            }
        }

        public string OpenFile()
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
