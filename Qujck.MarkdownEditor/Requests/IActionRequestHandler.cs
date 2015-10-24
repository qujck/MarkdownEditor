using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qujck.MarkdownEditor.Requests
{
    internal interface IActionRequestHandler<TCommand> where TCommand : IActionRequest
    {
        void Run(TCommand command);
    }
}
