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
        public static string Execute(this IQueryHandler<Styles, string> handler)
        {
            return handler.Execute(new Styles());
        }

        public sealed class Styles : IQuery<string>
        {
            internal Styles() { }
        }

        public static partial class Handlers
        {
            public sealed class StylesHandler : IQueryHandler<Styles, string>
            {
                private readonly IStringResourceProvider stringResourceProvider;

                public StylesHandler(IStringResourceProvider stringResourceProvider)
                {
                    this.stringResourceProvider = stringResourceProvider;
                }

                public string Execute(Styles query)
                {
                    string bootstrap = this.stringResourceProvider.Single("Content.bootstrap.min.css");
                    string site = this.stringResourceProvider.Single("Content.site.css");

                    return bootstrap + Environment.NewLine + 
                        site;
                }
            }
        }
    }
}
