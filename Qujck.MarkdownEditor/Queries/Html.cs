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
                private readonly IStringResourceProvider stringResourceProvider;

                public HtmlHandler(IStringResourceProvider stringResourceProvider)
                {
                    this.stringResourceProvider = stringResourceProvider;
                }

                public string Execute(Html query)
                {
                    string html = this.stringResourceProvider.One(Constants.Content.Layout);

                    return html;
                }
            }
        }
    }
}
