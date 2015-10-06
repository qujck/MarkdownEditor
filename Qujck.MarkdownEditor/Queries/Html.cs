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
        public static string Execute(this IQueryService<Html, string> handler)
        {
            return handler.Execute(new Html());
        }

        public sealed class Html : IQueryParameter<string>
        {
            internal Html() { }
        }

        public static partial class Handlers
        {
            public sealed class HtmlHandler : IQueryService<Html, string>
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
