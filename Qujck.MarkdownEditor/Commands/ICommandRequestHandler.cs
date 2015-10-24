﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qujck.MarkdownEditor.Commands
{
    internal interface ICommandRequestHandler<TCommand> where TCommand : ICommandRequest
    {
        void Run(TCommand command);
    }
}
