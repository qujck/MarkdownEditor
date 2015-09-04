using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qujck.MarkdownEditor.Infrastructure;
using Qujck.MarkdownEditor.Queries;

namespace Qujck.MarkdownEditor.Aspects
{
    public sealed class PrepareHtml : IQueryHandler<Query.Html, string>
    {
        private readonly IQueryHandler<Query.Html, string> decorated;
        private readonly IQueryHandler<Query.Styles, string> stylesQuery;
        private readonly IQueryHandler<Query.Scripts, string> scriptsQuery;

        public PrepareHtml(
            IQueryHandler<Query.Html, string> decorated,
            IQueryHandler<Query.Styles, string> styles,
            IQueryHandler<Query.Scripts, string> scripts)
        {
            this.decorated = decorated;
            this.stylesQuery = styles;
            this.scriptsQuery = scripts;
        }

        public string Execute(Query.Html query)
        {
            string result = this.decorated.Execute(query);

            var html = result
                .Replace("${style}", this.stylesQuery.Execute())
                .Replace("${script}", this.scriptsQuery.Execute());

            return html;
        }
    }
}
