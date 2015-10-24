using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qujck.MarkdownEditor.Infrastructure;

namespace Qujck.MarkdownEditor.Queries
{
    internal static partial class Query
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
                private readonly IStringResourceProvider stringResourceProvider;

                public ScriptsHandler(IStringResourceProvider stringResourceProvider)
                {
                    this.stringResourceProvider = stringResourceProvider;
                }

                public string Execute(Scripts query)
                {
                    string scripts = this.stringResourceProvider.One(Constants.Scripts.Marked);

                    return scripts;
                }
            }
        }
    }
}
