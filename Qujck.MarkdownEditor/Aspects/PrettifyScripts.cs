using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qujck.MarkdownEditor.Infrastructure;
using Qujck.MarkdownEditor.Queries;

namespace Qujck.MarkdownEditor.Aspects
{
    public sealed class PrettifyScripts : IQueryHandler<Query.Scripts, string>
    {
        private readonly IQueryHandler<Query.Scripts, string> decorated;

        public PrettifyScripts(IQueryHandler<Query.Scripts, string> decorated)
        {
            this.decorated = decorated;
        }

        public string Execute(Query.Scripts query)
        {
            string result = this.decorated.Execute(query);

            var scripts = new StringBuilder(result)
                .AppendResource("Scripts.Prettify.prettify.js")
                .AppendManyResources("Scripts.Prettify.lang-");

            return scripts.ToString();
        }
    }
}
