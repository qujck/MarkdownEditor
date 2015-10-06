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

        private readonly ICommandService<Command.WriteDocument> writeDocumentService;

        private readonly IQueryService<Query.Html, string> htmlQueryService;
        private readonly IQueryService<Query.Scripts, string> scriptsQueryService;
        private readonly IQueryService<Query.Styles, string> stylesQueryService;

        private readonly IStringResourceProvider stringResourceProvider;

        private CompositionRoot()
        {
            this.stringResourceProvider = new StringResourceProvider();

            this.scriptsQueryService = new PrettifyScripts(
                new Query.Handlers.ScriptsHandler(
                    this.stringResourceProvider),
                this.stringResourceProvider);

            this.stylesQueryService = new PrettifyStyles(
                new Query.Handlers.StylesHandler(
                    this.stringResourceProvider),
                this.stringResourceProvider);

            this.htmlQueryService = new PrepareHtml(
                new Query.Handlers.HtmlHandler(
                    this.stringResourceProvider),
                this.stylesQueryService,
                this.scriptsQueryService);

            this.writeDocumentService = new ImagePathFixer(
                new PrettifyInvoke(
                    new Command.Handlers.WriteDocumentHandler()));
        }

        public T Resolve<T>() where T : class
        {
            return this.Resolve(typeof(T)) as T;
        }

        public object Resolve(Type serviceType)
        {
            if (serviceType == typeof(IQueryService<Query.Html, string>))
            {
                return this.htmlQueryService;
            }
            else if (serviceType == typeof(ICommandService<Command.WriteDocument>))
            {
                return this.writeDocumentService;
            }
            else if (serviceType == typeof(IStringResourceProvider))
            {
                return this.stringResourceProvider;
            }

            throw new InvalidOperationException();
        }
    }
}
