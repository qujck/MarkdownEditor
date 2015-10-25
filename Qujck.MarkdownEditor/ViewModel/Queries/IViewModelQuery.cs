using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qujck.MarkdownEditor.ViewModel.Core;

namespace Qujck.MarkdownEditor.ViewModel.Queries
{
    internal interface IViewModelQuery<TViewModelParameter> where TViewModelParameter : IViewModelParameter
    {
        bool Execute(TViewModelParameter viewModelParameter);
    }
}
