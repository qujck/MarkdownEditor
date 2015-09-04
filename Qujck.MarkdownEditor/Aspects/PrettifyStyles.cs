using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qujck.MarkdownEditor.Infrastructure;
using Qujck.MarkdownEditor.Queries;

namespace Qujck.MarkdownEditor.Aspects
{
    public sealed class PrettifyStyles : IQueryHandler<Query.Styles, string>
    {
        private readonly IQueryHandler<Query.Styles, string> decorated;

        public PrettifyStyles(IQueryHandler<Query.Styles, string> decorated)
        {
            this.decorated = decorated;
        }

        public string Execute(Query.Styles query)
        {
            string result = this.decorated.Execute(query);

            var scripts = new StringBuilder(result)
                .AppendResource("Content.stackoverflow.css");

            return scripts.ToString();
        }
    }
}
