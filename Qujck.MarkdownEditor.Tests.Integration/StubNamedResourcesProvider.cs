using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qujck.MarkdownEditor.Requests;

namespace Qujck.MarkdownEditor.Tests.Integration
{
    internal class StubNamedResourcesProvider :
        IStringRequestHandler<Strings.NamedResources>
    {
        private readonly string response;

        public StubNamedResourcesProvider(string response)
        {
            this.response = response;
        }

        public string Execute(Strings.NamedResources query)
        {
            return this.response;
        }
    }
}
