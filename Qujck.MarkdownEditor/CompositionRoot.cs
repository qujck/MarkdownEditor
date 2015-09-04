using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qujck.MarkdownEditor.Commands;
using Qujck.MarkdownEditor.Queries;
using Qujck.MarkdownEditor.Aspects;

namespace Qujck.MarkdownEditor
{
    public class CompositionRoot
    {
        public T Resolve<T>() where T : class
        {
            if (typeof(T) == typeof(IQueryHandler<Query.Scripts, string>))
                return new Query.Handlers.ScriptsHandler() as T;
            else if (typeof(T) == typeof(IQueryHandler<Query.Styles, string>))
                return new Query.Handlers.StylesHandler() as T;
            else if (typeof(T) == typeof(IQueryHandler<Query.Html, string>))
                return new Query.Handlers.HtmlHandler() as T;
            else if (typeof(T) == typeof(IQueryHandler<Query.Markdown, string>))
                return new MarkdownPrettyPrint(new Query.Handlers.MarkdownHandler()) as T;

            throw new InvalidOperationException();
        }
    }
}
