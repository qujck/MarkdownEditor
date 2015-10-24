using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qujck.MarkdownEditor.Queries
{
    internal interface IStringRequestHandler<TQuery> where TQuery : IStringRequest
    {
        string Execute(TQuery query);
    }
}
