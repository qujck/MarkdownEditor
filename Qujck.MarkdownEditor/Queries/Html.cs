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

        internal static partial class Handlers
        {
            public sealed class HtmlHandler : IQueryHandler<Html, string>
            {
                public string Execute(Html query)
                {
                    var html = ResourceHelpers.ReadResource("Qujck.MarkdownEditor.Content.layout.html");

                    return html.ToString();
                }
            }
        }
    }
}
