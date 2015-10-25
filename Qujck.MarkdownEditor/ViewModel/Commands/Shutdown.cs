using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Qujck.MarkdownEditor.ViewModel.Core;

namespace Qujck.MarkdownEditor.ViewModel.Commands
{
    internal sealed class Shutdown : DynamicViewModelParameter
    {
        public Shutdown(DocumentViewModel viewModel) : base(viewModel)
        {
        }
    }

    internal sealed class ShutdownHandler : IViewModelCommand<Shutdown>
    {
        public void Run(Shutdown viewModelParameter)
        {
            Application.Current.Shutdown();
        }
    }
}
