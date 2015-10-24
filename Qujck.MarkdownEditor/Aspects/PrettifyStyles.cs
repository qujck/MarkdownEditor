using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qujck.MarkdownEditor.Infrastructure;
using Qujck.MarkdownEditor.Queries;

namespace Qujck.MarkdownEditor.Aspects
{
    internal sealed class PrettifyStyles : IStringRequestHandler<Query.Styles>
    {
        private readonly IStringRequestHandler<Query.Styles> decorated;
        private readonly IStringResourceProvider stringResourceProvider;

        public PrettifyStyles(
            IStringRequestHandler<Query.Styles> decorated,
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
