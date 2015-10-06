using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using Qujck.MarkdownEditor.Infrastructure;
using Qujck.MarkdownEditor.Commands;
using Qujck.MarkdownEditor.Queries;
using Qujck.MarkdownEditor.Aspects;

namespace Qujck.MarkdownEditor
{
    public sealed class CompositionRoot
    {
        private static CompositionRoot _instance = new CompositionRoot();
        public static CompositionRoot Instance
        {
            get
            {
                return _instance;
            }
        }

        private readonly ICommandHandler<Command.WriteDocument> writeDocumentHandler;

        private readonly IQueryHandler<Query.Html, string> htmlHandler;
        private readonly IQueryHandler<Query.Scripts, string> scriptsHandler;
        private readonly IQueryHandler<Query.Styles, string> stylesHandler;

        private readonly IStringResourceProvider stringResourceProvider;

        public CompositionRoot()
        {
            this.stringResourceProvider = new StringResourceProvider();

            this.scriptsHandler = new PrettifyScripts(
                new Query.Handlers.ScriptsHandler(
                    this.stringResourceProvider),
                this.stringResourceProvider);

            this.stylesHandler = new PrettifyStyles(
                new Query.Handlers.StylesHandler(
                    this.stringResourceProvider),
                this.stringResourceProvider);

            this.htmlHandler = new PrepareHtml(
                new Query.Handlers.HtmlHandler(
                    this.stringResourceProvider),
                this.stylesHandler,
                this.scriptsHandler);

            this.writeDocumentHandler = new ImagePathFixer(
                new PrettifyInvoke(
                    new Command.Handlers.WriteDocumentHandler()));
        }

        public T Resolve<T>() where T : class
        {
            return this.Resolve(typeof(T)) as T;
        }

        public object Resolve(Type serviceType)
        {
            if (serviceType == typeof(IQueryHandler<Query.Html, string>))
            {
                return this.htmlHandler;
            }
            else if (serviceType == typeof(ICommandHandler<Command.WriteDocument>))
            {
                return this.writeDocumentHandler;
            }
            else if (serviceType == typeof(IStringResourceProvider))
            {
                return this.stringResourceProvider;
            }

            throw new InvalidOperationException();
        }
    }
}
