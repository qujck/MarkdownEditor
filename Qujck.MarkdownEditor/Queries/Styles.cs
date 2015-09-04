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
                public string Execute(Styles query)
                {
                    var csss = new StringBuilder()
                        .AppendResource("Content.bootstrap.min.css")
                        .AppendResource("Content.bootstrap-theme.min.css")
                        .AppendResource("Content.site.css");

                    return csss.ToString();
                }
            }
        }
    }
}
