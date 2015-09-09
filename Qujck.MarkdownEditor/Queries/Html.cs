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
        public static string Execute(this IQueryHandler<Html, string> handler)
        {
            return handler.Execute(new Html());
        }

        public sealed class Html : IQuery<string>
        {
            internal Html() { }
        }

        public static partial class Handlers
        {
            public sealed class HtmlHandler : IQueryHandler<Html, string>
            {
                private readonly IStringResourceProvider stringResourceProvider;

                public HtmlHandler(IStringResourceProvider stringResourceProvider)
                {
                    this.stringResourceProvider = stringResourceProvider;
                }

                public string Execute(Html query)
                {
                    string html = this.stringResourceProvider.Single(Constants.Content.Layout);

                    return html;
                }
            }
        }
    }
}
