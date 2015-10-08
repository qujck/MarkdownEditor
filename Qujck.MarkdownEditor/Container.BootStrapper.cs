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
    public sealed partial class Container
    {
        private static DependencyResolver _instance;
        static Container()
        {
            _instance = DependencyResolver.Build();
        }

        public static IStringResourceProvider StringResourceProvider
        {
            get
            {
                return _instance.Resolve<IStringResourceProvider>();
            }
        }

        private sealed class DependencyResolver
        {
            private ICommandHandler<Command.RenderMarkdown> renderMarkdownHandler;
            private IQueryHandler<Query.Html, string> htmlQueryHandler;
            private IQueryHandler<Query.Scripts, string> scriptsQueryHandler;
            private IQueryHandler<Query.Styles, string> stylesQueryHandler;
            private IStringResourceProvider stringResourceProvider;

            private DependencyResolver()
            {
            }

            internal static DependencyResolver Build()
            {
                return new DependencyResolver()
                    .RegisterProviders()
                    .RegisterCommandHandlers()
                    .RegisterQueryHandlers();
            }

            private DependencyResolver RegisterProviders()
            {
                this.stringResourceProvider = new StringResourceProvider();

                return this;
            }

            private DependencyResolver RegisterCommandHandlers()
            {
                this.renderMarkdownHandler = new ImagePathFixer(
                    new PrettifyInvoke(
                        new Command.Handlers.RenderMarkdownHandler()));

                return this;
            }

            private DependencyResolver RegisterQueryHandlers()
            {
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

                return this;
            }

            internal T Resolve<T>() where T : class
            {
                return this.Resolve(typeof(T)) as T;
            }

            internal object Resolve(Type serviceType)
            {
                if (serviceType == typeof(IQueryHandler<Query.Html, string>))
                {
                    return this.htmlQueryHandler;
                }
                else if (serviceType == typeof(ICommandHandler<Command.RenderMarkdown>))
                {
                    return this.renderMarkdownHandler;
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