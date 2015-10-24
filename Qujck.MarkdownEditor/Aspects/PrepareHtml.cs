using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qujck.MarkdownEditor.Infrastructure;
using Qujck.MarkdownEditor.Requests;

namespace Qujck.MarkdownEditor.Aspects
{
    internal sealed class PrepareHtml : IStringRequestHandler<Strings.Html>
    {
        private readonly IStringRequestHandler<Strings.Html> decorated;
        private readonly IStringRequestHandler<Strings.Styles> stylesQuery;
        private readonly IStringRequestHandler<Strings.Scripts> scriptsQuery;

        public PrepareHtml(
            IStringRequestHandler<Strings.Html> decorated,
            IStringRequestHandler<Strings.Styles> styles,
            IStringRequestHandler<Strings.Scripts> scripts)
        {
            this.decorated = decorated;
            this.stylesQuery = styles;
            this.scriptsQuery = scripts;
        }

        public string Execute(Strings.Html query)
        {
            string result = this.decorated.Execute(query);

            var html = result
                .Replace("${style}", this.stylesQuery.Execute())
                .Replace("${script}", this.scriptsQuery.Execute());

            return html;
        }
    }
}
