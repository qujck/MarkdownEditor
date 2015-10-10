using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qujck.MarkdownEditor.Infrastructure;

namespace Qujck.MarkdownEditor.ViewModel.Commands
{
    internal sealed class NewFile : ViewModelParameter
    {
        public NewFile(DocumentViewModel viewModel) : base(viewModel)
        {
        }
    }

    internal sealed class NewFileHandler : IViewModelCommand<NewFile>
    {
        public void Execute(NewFile viewModelParameter)
        {
            viewModelParameter.ViewModel[Constants.DocumentViewModel.OpeningText] = string.Empty;
            viewModelParameter.ViewModel[Constants.DocumentViewModel.CurrentText] = string.Empty;
            viewModelParameter.ViewModel[Constants.DocumentViewModel.FilePath] = string.Empty;
        }
    }
}
