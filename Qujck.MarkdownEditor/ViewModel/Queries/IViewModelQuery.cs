using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qujck.MarkdownEditor.Infrastructure;

namespace Qujck.MarkdownEditor.ViewModel.Queries
{
    internal interface IViewModelQuery<TViewModelParameter> where TViewModelParameter : ViewModelParameter
    {
        bool CanExecute(TViewModelParameter viewModelParameter);
    }
}
