using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qujck.MarkdownEditor.Infrastructure;
using Qujck.MarkdownEditor.Queries;

namespace Qujck.MarkdownEditor.Aspects
{
    public sealed class ImagePathFixer : IQueryHandler<Query.MarkdownToHtml, string>
    {
        private readonly IQueryHandler<Query.MarkdownToHtml, string> decorated;

        public ImagePathFixer(IQueryHandler<Query.MarkdownToHtml, string> decorated)
        {
            this.decorated = decorated;
        }

        public string Execute(Query.MarkdownToHtml query)
        {
            string markdown = query.Text.Replace(
                "![image](~",
                string.Format("![image]({0}", ".."));

            string html = this.decorated.Execute(markdown);

            return html;
        }
    }
}
