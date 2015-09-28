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
        private readonly IStringResourceProvider stringResourceProvider;

        public PrettifyStyles(
            IQueryHandler<Query.Styles, string> decorated,
            IStringResourceProvider stringResourceProvider)
        {
            this.decorated = decorated;
            this.stringResourceProvider = stringResourceProvider;
        }

        public string Execute(Query.Styles query)
        {
            string result = this.decorated.Execute(query);

            var scripts = this.stringResourceProvider.One("Content.stackoverflow.css");

            return result + Environment.NewLine + 
                scripts;
        }
    }
}
