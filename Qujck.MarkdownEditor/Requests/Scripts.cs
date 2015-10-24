using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qujck.MarkdownEditor.Infrastructure;

namespace Qujck.MarkdownEditor.Requests
{
    internal static partial class Strings
    {
        internal static string Execute(this IStringRequestHandler<Scripts> handler)
        {
            return handler.Execute(new Scripts());
        }

        internal sealed class Scripts : IStringRequest
        {
            internal Scripts() { }
        }

        internal static partial class Handlers
        {
            internal sealed class ScriptsHandler : IStringRequestHandler<Scripts>
            {
                private readonly IStringRequestHandler<NamedResources> namedResources;

                public ScriptsHandler(IStringRequestHandler<NamedResources> namedResources)
                {
                    this.namedResources = namedResources;
                }

                public string Execute(Scripts query)
                {
                    string scripts = this.namedResources.Execute(Constants.Scripts.Marked);

                    return scripts;
                }
            }
        }
    }
}
