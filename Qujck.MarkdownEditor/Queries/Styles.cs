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
        internal static string Execute(this IStringRequestHandler<Styles> handler)
        {
            return handler.Execute(new Styles());
        }

        internal sealed class Styles : IStringRequest
        {
            internal Styles() { }
        }

        internal static partial class Handlers
        {
            internal sealed class StylesHandler : IStringRequestHandler<Styles>
            {
                private readonly IStringResourceProvider stringResourceProvider;

                public StylesHandler(IStringResourceProvider stringResourceProvider)
                {
                    this.stringResourceProvider = stringResourceProvider;
                }

                public string Execute(Styles query)
                {
                    string bootstrap = this.stringResourceProvider.One(Constants.Content.Bootstrap);
                    string site = this.stringResourceProvider.One(Constants.Content.SiteCss);

                    return bootstrap + Environment.NewLine + 
                        site;
                }
            }
        }
    }
}
