using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qujck.MarkdownEditor.ViewModel.Queries
{
    internal sealed class CanSaveFile : ViewModelParameter
    {
        public CanSaveFile(DocumentViewModel viewModel) : base(viewModel)
        {
        }
    }

    internal sealed class CanSaveFileHandler : IViewModelQuery<CanSaveFile>
    {
        public bool CanExecute(CanSaveFile viewModelParameter)
        {
            return (string)viewModelParameter.ViewModel[Constants.DocumentViewModel.OpeningText] !=
                (string)viewModelParameter.ViewModel[Constants.DocumentViewModel.CurrentText];
        }
    }
}
