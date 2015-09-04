using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Strike.IE;
using Qujck.MarkdownEditor.Infrastructure;

namespace Qujck.MarkdownEditor.Queries
{
    public static partial class Query
    {
        public static string Execute(this IQueryHandler<MarkdownToHtml, string> handler, string text)
        {
            return handler.Execute(new MarkdownToHtml(text));
        }

        public sealed class MarkdownToHtml : IQuery<string>
        {
            internal MarkdownToHtml(string text)
            {
                this.Text = text;
            }

            public string Text { get; private set; }
        }

        public static partial class Handlers
        {
            public sealed class MarkdownToHtmlHandler : IQueryHandler<MarkdownToHtml, string>
            {
                public string Execute(MarkdownToHtml query)
                {
                    var transformer = new Markdownify();

                    string markdown = transformer.Transform(query.Text);

                    return markdown;
                }
            }
        }
    }
}
