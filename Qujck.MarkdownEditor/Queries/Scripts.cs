using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qujck.MarkdownEditor.Infrastructure;

namespace Qujck.MarkdownEditor.Queries
{
    public static partial class Query
    {
        public static string Execute(this IQueryHandler<Scripts, string> handler)
        {
            return handler.Execute(new Scripts());
        }

        public sealed class Scripts : IQuery<string>
        {
            internal Scripts() { }
        }

        public static partial class Handlers
        {
            public sealed class ScriptsHandler : IQueryHandler<Scripts, string>
            {
                private readonly IStringResourceProvider stringResourceProvider;

                public ScriptsHandler(IStringResourceProvider stringResourceProvider)
                {
                    this.stringResourceProvider = stringResourceProvider;
                }

                public string Execute(Scripts query)
                {
                    string scripts = this.stringResourceProvider.Single(Constants.Scripts.Marked);

                    return scripts;
                }
            }
        }
    }
}
