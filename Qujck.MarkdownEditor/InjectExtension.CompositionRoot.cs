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
    public partial class InjectExtension
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
            private readonly IQueryHandler<Query.Html, string> htmlQueryHandler;
            private readonly IQueryHandler<Query.Scripts, string> scriptsQueryHandler;
            private readonly IQueryHandler<Query.Styles, string> stylesQueryHandler;
            private readonly IStringResourceProvider stringResourceProvider;

            private CompositionRoot()
            {
                this.stringResourceProvider = new StringResourceProvider();

                this.scriptsQueryHandler = new PrettifyScripts(
                    new Query.Handlers.ScriptsHandler(
                        this.stringResourceProvider),
                    this.stringResourceProvider);

                this.stylesQueryHandler = new PrettifyStyles(
                    new Query.Handlers.StylesHandler(
                        this.stringResourceProvider),
                    this.stringResourceProvider);

                this.htmlQueryHandler = new PrepareHtml(
                    new Query.Handlers.HtmlHandler(
                        this.stringResourceProvider),
                    this.stylesQueryHandler,
                    this.scriptsQueryHandler);

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
                    return this.htmlQueryHandler;
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
}