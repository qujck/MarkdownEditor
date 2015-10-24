using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qujck.MarkdownEditor.Requests;

namespace Qujck.MarkdownEditor.Tests.Unit
{
    internal class StubNamedResourcesProvider :
        IStringRequestHandler<Strings.NamedResources>
    {
        public string Execute(Strings.NamedResources query)
        {
            return string.Join(Environment.NewLine, query.Names);
        }
    }
}
