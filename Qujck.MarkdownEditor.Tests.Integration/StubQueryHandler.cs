using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qujck.MarkdownEditor.Queries;

namespace Qujck.MarkdownEditor.Tests.Integration
{
    public class StubQueryHandler<TQuery, TResult> : 
        IQueryHandler<TQuery, TResult> where TQuery : IQueryParameter<TResult>
    {
        private readonly Func<TResult> result;

        public StubQueryHandler(Func<TResult> result)
        {
            this.result = result;
        }

        public TResult Execute(TQuery query)
        {
            return result();
        }
    }
}
