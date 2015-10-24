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

            string prettify = this.namedResources.Execute(Constants.Content.Scripts.Prettify);
            string prettifyLang = this.prefixedResources.Execute(Constants.Content.Scripts.PrettifyLang);

            return result + Environment.NewLine +
                prettify + Environment.NewLine +
                prettifyLang + Environment.NewLine +
                Constants.Content.Scripts.EmbeddedPrettifyAction;
        }
    }
}
