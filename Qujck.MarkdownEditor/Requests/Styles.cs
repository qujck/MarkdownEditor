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
                private readonly IStringRequestHandler<NamedResources> namedResources;

                public StylesHandler(IStringRequestHandler<NamedResources> namedResources)
                {
                    this.namedResources = namedResources;
                }

                public string Execute(Styles query)
                {
                    string bootstrap = this.namedResources.Execute(Constants.Content.Bootstrap);
                    string site = this.namedResources.Execute(Constants.Content.SiteCss);

                    return bootstrap + Environment.NewLine + 
                        site;
                }
            }
        }
    }
}
