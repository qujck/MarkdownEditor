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
        internal static string Execute(this IStringRequestHandler<Html> handler)
        {
            return handler.Execute(new Html());
        }

        internal sealed class Html : IStringRequest
        {
            internal Html() { }
        }

        internal static partial class Handlers
        {
            internal sealed class HtmlHandler : IStringRequestHandler<Html>
            {
                private readonly IStringRequestHandler<NamedResources> namedResources;

                public HtmlHandler(IStringRequestHandler<NamedResources> namedResources)
                {
                    this.namedResources = namedResources;
                }

                public string Execute(Html query)
                {
                    string html = this.namedResources.Execute(Constants.Content.Layout);

                    return html;
                }
            }
        }
    }
}
