using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qujck.MarkdownEditor.Infrastructure;

namespace Qujck.MarkdownEditor.ViewModel.Commands
{
    internal sealed class NextView : ViewModelParameter
    {
        public NextView(DocumentViewModel viewModel) : base(viewModel)
        {
        }
    }

    internal sealed class NextViewHandler : IViewModelCommand<NextView>
    {
        public void Execute(NextView viewModelParameter)
        {
            int next = WhatsNext(viewModelParameter.ViewModel[Constants.DocumentViewModel.CurrentView]);
            viewModelParameter.ViewModel[Constants.DocumentViewModel.CurrentView] = next;
            viewModelParameter.ViewModel.Update(Constants.DocumentViewModel.Views[next]);
        }

        private static int WhatsNext(object currentView)
        {
            int next = (int)currentView + 1;
            return Constants.DocumentViewModel.Views.Length == next
                ? 0
                : next;
        }
    }
}
