using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qujck.MarkdownEditor.Infrastructure;
using Qujck.MarkdownEditor.Requests;

namespace Qujck.MarkdownEditor.Aspects
{
    internal sealed class PrettifyScripts : IStringRequestHandler<Strings.Scripts>
    {
        const string prettifyCodeSamples = 
@"function prettifyCodeSamples() {
    var text = document.getElementById('content').innerHTML;
    var result = text.replace(/<pre>/gi, '<pre class=""prettyprint"">');
    document.getElementById('content').innerHTML = result;
    prettyPrint();
}";

        private readonly IStringRequestHandler<Strings.Scripts> decorated;
        private readonly IStringRequestHandler<Strings.NamedResources> namedResources;
        private readonly IStringRequestHandler<Strings.PrefixedResources> prefixedResources;

        public PrettifyScripts(
            IStringRequestHandler<Strings.Scripts> decorated,
            IStringRequestHandler<Strings.NamedResources> namedResources,
            IStringRequestHandler<Strings.PrefixedResources> prefixedResources)
        {
            this.decorated = decorated;
            this.namedResources = namedResources;
            this.prefixedResources = prefixedResources;
        }

        public string Execute(Strings.Scripts query)
        {
            string result = this.decorated.Execute(query);

            string prettify = this.namedResources.Execute("Scripts.Prettify.prettify.js");
            string prettifyLang = this.prefixedResources.Execute("Scripts.Prettify.lang-");

            return result + Environment.NewLine +
                prettify + Environment.NewLine +
                prettifyLang + Environment.NewLine +
                prettifyCodeSamples;
        }
    }
}
