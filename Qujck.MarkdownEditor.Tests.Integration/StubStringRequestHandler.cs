using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qujck.MarkdownEditor.Requests;

namespace Qujck.MarkdownEditor.Tests.Integration
{
    internal class StubQueryHandler<TQuery> : 
        IStringRequestHandler<TQuery> where TQuery : IStringRequest
    {
        private readonly Func<string> result;

        public StubQueryHandler(Func<string> result)
        {
            this.result = result;
        }

        public string Execute(TQuery query)
        {
            return result();
        }
    }
}
