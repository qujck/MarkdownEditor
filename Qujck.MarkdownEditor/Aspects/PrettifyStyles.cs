using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qujck.MarkdownEditor.Infrastructure;
using Qujck.MarkdownEditor.Requests;

namespace Qujck.MarkdownEditor.Aspects
{
    internal sealed class PrettifyStyles : IStringRequestHandler<Strings.Styles>
    {
        private readonly IStringRequestHandler<Strings.Styles> decorated;
        private readonly IStringRequestHandler<Strings.NamedResources> namedResources;

        public PrettifyStyles(
            IStringRequestHandler<Strings.Styles> decorated,
            IStringRequestHandler<Strings.NamedResources> namedResources)
        {
            this.decorated = decorated;
            this.namedResources = namedResources;
        }

        public string Execute(Strings.Styles query)
        {
            string result = this.decorated.Execute(query);

            var scripts = this.namedResources.Execute(Constants.Content.StackOverflowCss);

            return result + Environment.NewLine + 
                scripts;
        }
    }
}
