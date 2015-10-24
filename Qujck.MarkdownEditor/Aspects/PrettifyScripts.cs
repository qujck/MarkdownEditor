﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qujck.MarkdownEditor.Infrastructure;
using Qujck.MarkdownEditor.Queries;

namespace Qujck.MarkdownEditor.Aspects
{
    internal sealed class PrettifyScripts : IStringRequestHandler<Query.Scripts>
    {
        const string prettifyCodeSamples = 
@"function prettifyCodeSamples() {
    var text = document.getElementById('content').innerHTML;
    var result = text.replace(/<pre>/gi, '<pre class=""prettyprint"">');
    document.getElementById('content').innerHTML = result;
    prettyPrint();
}";

        private readonly IStringRequestHandler<Query.Scripts> decorated;
        private readonly IStringResourceProvider stringResourceProvider;

        public PrettifyScripts(
            IStringRequestHandler<Query.Scripts> decorated,
            IStringResourceProvider stringResourceProvider)
        {
            this.decorated = decorated;
            this.stringResourceProvider = stringResourceProvider;
        }

        public string Execute(Query.Scripts query)
        {
            string result = this.decorated.Execute(query);

            string prettify = this.stringResourceProvider.One("Scripts.Prettify.prettify.js");
            string prettifyLang = this.stringResourceProvider.Many("Scripts.Prettify.lang-");

            return result + Environment.NewLine +
                prettify + Environment.NewLine +
                prettifyLang + Environment.NewLine +
                prettifyCodeSamples;
        }
    }
}
