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
        private readonly ICommandHandler<Command.WriteDocument> writeDocumentHandler;

        private readonly IQueryHandler<Query.Html, string> htmlHandler;
        private readonly IQueryHandler<Query.Markdown, string> markdownHandler;
        private readonly IQueryHandler<Query.Scripts, string> scriptsHandler;
        private readonly IQueryHandler<Query.Styles, string> stylesHandler;

        public CompositionRoot()
        {
            this.markdownHandler = new PrettifyMarkdown(new Query.Handlers.MarkdownHandler());
            this.scriptsHandler = new PrettifyScripts(new Query.Handlers.ScriptsHandler());
            this.stylesHandler = new PrettifyStyles(new Query.Handlers.StylesHandler());

            this.htmlHandler = new PrepareHtml(
                new Query.Handlers.HtmlHandler(),
                this.stylesHandler,
                this.scriptsHandler);

            this.writeDocumentHandler = new PrettifyInvoke(
                new Command.Handlers.WriteDocumentHandler(
                    this.htmlHandler));
        }

        public T Resolve<T>() where T : class
        { 
            if (typeof(T) == typeof(IQueryHandler<Query.Markdown, string>))
            {
                return this.markdownHandler as T;
            }
            else if (typeof(T) == typeof(ICommandHandler<Command.WriteDocument>))
            {
                return this.writeDocumentHandler as T;
            }

            throw new InvalidOperationException();
        }
    }
}
