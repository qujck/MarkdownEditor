using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qujck.MarkdownEditor.Infrastructure;
using Qujck.MarkdownEditor.Queries;

namespace Qujck.MarkdownEditor.Aspects
{
    public sealed class PrepareHtml : IQueryService<Query.Html, string>
    {
        private readonly IQueryService<Query.Html, string> decorated;
        private readonly IQueryService<Query.Styles, string> stylesQuery;
        private readonly IQueryService<Query.Scripts, string> scriptsQuery;

        public PrepareHtml(
            IQueryService<Query.Html, string> decorated,
            IQueryService<Query.Styles, string> styles,
            IQueryService<Query.Scripts, string> scripts)
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
