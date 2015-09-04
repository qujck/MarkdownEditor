using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qujck.MarkdownEditor.Infrastructure;
using Qujck.MarkdownEditor.Queries;

namespace Qujck.MarkdownEditor.Aspects
{
    public sealed class PrettifyMarkdown : IQueryHandler<Query.Markdown, string>
    {
        private readonly IQueryHandler<Query.Markdown, string> decorated;

        public PrettifyMarkdown(IQueryHandler<Query.Markdown, string> decorated)
        {
            this.decorated = decorated;
        }

        public string Execute(Query.Markdown query)
        {
            string result = this.decorated.Execute(query);

            result = result.Replace("<pre", "<pre class=\"prettyprint\"");

            return result;
        }
    }
}
