using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qujck.MarkdownEditor.Infrastructure;
using Qujck.MarkdownEditor.Queries;

namespace Qujck.MarkdownEditor.Aspects
{
    internal sealed class PrepareHtml : IStringRequestHandler<Query.Html>
    {
        private readonly IStringRequestHandler<Query.Html> decorated;
        private readonly IStringRequestHandler<Query.Styles> stylesQuery;
        private readonly IStringRequestHandler<Query.Scripts> scriptsQuery;

        public PrepareHtml(
            IStringRequestHandler<Query.Html> decorated,
            IStringRequestHandler<Query.Styles> styles,
            IStringRequestHandler<Query.Scripts> scripts)
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
