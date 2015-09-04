using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Strike.IE;

namespace Qujck.MarkdownEditor.Queries
{
    public static partial class Query
    {
        public static string Execute(this IQueryHandler<Markdown, string> handler, string text)
        {
            return handler.Execute(new Markdown(text));
        }

        public sealed class Markdown : IQuery<string>
        {
            internal Markdown(string text)
            {
                this.Text = text;
            }

            public string Text { get; private set; }
        }

        internal static partial class Handlers
        {
            public sealed class MarkdownHandler : IQueryHandler<Markdown, string>
            {
                public string Execute(Markdown query)
                {
                    var content = query.Text.Replace(
                        "![image](~",
                        string.Format("![image]({0}", ""));

                    var transformer = new Markdownify();

                    string markdown = transformer.Transform(content);

                    return markdown;
                }
            }
        }
    }
}
