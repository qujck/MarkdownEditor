using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qujck.MarkdownEditor.ViewModel.Core;

namespace Qujck.MarkdownEditor.ViewModel.Commands
{
    internal sealed class NewFile : DynamicViewModelParameter
    {
        public NewFile(DocumentViewModel viewModel) : base(viewModel)
        {
        }
    }

    internal sealed class NewFileHandler : IViewModelCommand<NewFile>
    {
        public void Run(NewFile viewModelParameter)
        {
            viewModelParameter[Constants.DocumentViewModel.OpeningText] = string.Empty;
            viewModelParameter[Constants.DocumentViewModel.CurrentText] = string.Empty;
            viewModelParameter[Constants.DocumentViewModel.FilePath] = string.Empty;
        }
    }
}
