using Qujck.MarkdownEditor.Infrastructure;
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
        public bool Execute(CanSaveFile viewModelParameter)
        {
            string openingText = (string)viewModelParameter[Constants.DocumentViewModel.OpeningText];
            string currentText = (string)viewModelParameter[Constants.DocumentViewModel.CurrentText];


            return openingText != currentText;
        }
    }
}
