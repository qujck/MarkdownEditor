using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qujck.MarkdownEditor.Infrastructure;

namespace Qujck.MarkdownEditor.ViewModel.Commands
{
    internal sealed class PreviousView : ViewModelParameter
    {
        public PreviousView(DocumentViewModel viewModel) : base(viewModel)
        {
        }
    }

    internal sealed class PreviousViewHandler : IViewModelCommand<PreviousView>
    {
        public void Run(PreviousView viewModelParameter)
        {
            int prior = WhatsPrior(viewModelParameter.ViewModel[Constants.DocumentViewModel.CurrentView]);
            viewModelParameter.ViewModel[Constants.DocumentViewModel.CurrentView] = prior;
            viewModelParameter.ViewModel.Update(Constants.DocumentViewModel.Views[prior]);
        }

        private static int WhatsPrior(object currentView)
        {
            int prior = (int)currentView - 1;
            return prior < 0
                ? Constants.DocumentViewModel.Views.Length - 1
                : prior;
        }
    }
}
