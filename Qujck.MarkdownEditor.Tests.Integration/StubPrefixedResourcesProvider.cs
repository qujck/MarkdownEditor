using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qujck.MarkdownEditor.Requests;

namespace Qujck.MarkdownEditor.Tests.Integration
{
    internal class StubPrefixedResourcesProvider :
        IStringRequestHandler<Strings.PrefixedResources>
    {
        private readonly string response;

        public StubPrefixedResourcesProvider(string response)
        {
            this.response = response;
        }

        public string Execute(Strings.PrefixedResources query)
        {
            return this.response;
        }
    }
}