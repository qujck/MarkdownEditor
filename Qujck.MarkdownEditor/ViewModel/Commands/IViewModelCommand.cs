﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qujck.MarkdownEditor.ViewModel.Commands
{
    internal interface IViewModelCommand<TViewModelParameter> where TViewModelParameter : IViewModelParameter
    {
        void Run(TViewModelParameter viewModelParameter);
    }
}
