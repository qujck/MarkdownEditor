﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qujck.MarkdownEditor.Actions
{
    internal interface IActionRequestHandler<TCommand> where TCommand : IActionRequest
    {
        void Run(TCommand command);
    }
}
