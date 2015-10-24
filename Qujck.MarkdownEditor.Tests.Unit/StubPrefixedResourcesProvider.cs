using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qujck.MarkdownEditor.Requests;

namespace Qujck.MarkdownEditor.Tests.Unit
{
    internal class StubPrefixedResourcesProvider :
        IStringRequestHandler<Strings.PrefixedResources>
    {
        public string Execute(Strings.PrefixedResources query)
        {
            return string.Join(Environment.NewLine, query.Prefixes);
        }
    }
}