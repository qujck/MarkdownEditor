using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qujck.MarkdownEditor.Infrastructure;

namespace Qujck.MarkdownEditor.ViewModel
{
    internal abstract class ViewModelParameter
    {
        public ViewModelParameter(DynamicViewModel viewModel)
        {
            this.ViewModel = viewModel;
        }

        public DynamicViewModel ViewModel { get; private set; }
    }
}
