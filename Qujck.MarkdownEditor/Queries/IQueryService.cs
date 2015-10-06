using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qujck.MarkdownEditor.Queries
{
    public interface IQueryService<TQuery, TResult> where TQuery : IQueryParameter<TResult>
    {
        TResult Execute(TQuery query);
    }
}
