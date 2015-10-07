﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qujck.MarkdownEditor.Commands
{
    public interface ICommandHandler<TCommand> where TCommand : ICommandParameter
    {
        void Run(TCommand command);
    }
}
