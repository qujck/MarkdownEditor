using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public void Run(NextView viewModelParameter)
        {
            int next = WhatsNext(viewModelParameter[Constants.DocumentViewModel.CurrentView]);
            viewModelParameter[Constants.DocumentViewModel.CurrentView] = next;
            viewModelParameter.DynamicViewModel.Update(Constants.DocumentViewModel.Views[next]);
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
