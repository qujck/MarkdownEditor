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
using Qujck.MarkdownEditor.ViewModel;
using Qujck.MarkdownEditor.ViewModel.Commands;
using Qujck.MarkdownEditor.ViewModel.Queries;

namespace Qujck.MarkdownEditor.Infrastructure
{
    public static class BootStrapper
    {
        internal static DependencyResolver Resolver { get; private set; }

        static BootStrapper()
        {
            Resolver = DependencyResolver.Build();
        }

        internal static bool ExecuteViewQuery(string name, DynamicViewModel viewModel)
        {
            var handler = Resolver.ResolveViewModelQuery(name);
            dynamic parameter = Activator.CreateInstance(
                handler.Item1,
                viewModel);
            return ((dynamic)handler.Item2).CanExecute(parameter);
        }

        internal static void ExecuteViewCommand(string name, DynamicViewModel viewModel)
        {
            var handler = Resolver.ResolveViewModelCommand(name);
            dynamic parameter = Activator.CreateInstance(
                handler.Item1,
                viewModel);
            ((dynamic)handler.Item2).Execute(parameter);
        }

        internal sealed class DependencyResolver
        {
            private ICommandHandler<Command.RenderMarkdown> renderMarkdownHandler;
            private IQueryHandler<Query.Html, string> htmlQueryHandler;
            private IQueryHandler<Query.Scripts, string> scriptsQueryHandler;
            private IQueryHandler<Query.Styles, string> stylesQueryHandler;
            private IStringResourceProvider stringResourceProvider;
            private IEnumerable<object> viewModelCommands;
            private IEnumerable<object> viewModelQueries;

            private DependencyResolver()
            {
            }

            internal static DependencyResolver Build()
            {
                return new DependencyResolver()
                    .RegisterProviders()
                    .RegisterCommandHandlers()
                    .RegisterQueryHandlers()
                    .RegisterViewModelCommands()
                    .RegisterViewModelQueries();
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

            private DependencyResolver RegisterViewModelCommands()
            {
                this.viewModelCommands = new object[]
                {
                    new NextViewHandler(),
                    new PreviousViewHandler(),
                    new OpenFileViewHandler(),
                    new SaveFileHandler()
                };

                return this;
            }

            private DependencyResolver RegisterViewModelQueries()
            {
                this.viewModelQueries = new object[]
                {
                    new CanSaveFileHandler()
                };

                return this;
            }

            internal T Resolve<T>() where T : class
            {
                return this.Resolve(typeof(T)) as T;
            }

            internal object Resolve(Type serviceType)
            {
                if (serviceType == typeof(IStringResourceProvider))
                {
                    return this.stringResourceProvider;
                }
                else if (serviceType == typeof(IQueryHandler<Query.Html, string>))
                {
                    return this.htmlQueryHandler;
                }
                else if (serviceType == typeof(ICommandHandler<Command.RenderMarkdown>))
                {
                    return this.renderMarkdownHandler;
                }

                throw new InvalidOperationException();
            }

            internal Tuple<Type, object> ResolveViewModelCommand(string name)
            {
                return this.FindViewModelHandler(
                    this.viewModelCommands, 
                    typeof(IViewModelCommand<>),
                    name);
            }

            internal Tuple<Type, object> ResolveViewModelQuery(string name)
            {
                return this.FindViewModelHandler(
                    this.viewModelQueries,
                    typeof(IViewModelQuery<>),
                    name);
            }

            private Tuple<Type, object> FindViewModelHandler(IEnumerable<object> handlers, Type service, string name)
            {
                var found =
                    from command in handlers
                    let type = command.GetType()
                    let @interface = type.GetInterface(service.Name)
                    let typename = @interface.GenericTypeArguments[0].FullName
                    where typename.EndsWith(string.Format(".{0}", name))
                    select new Tuple<Type, object>(
                        @interface.GenericTypeArguments[0],
                        command);

                switch (found.Count())
                {
                    case 0:
                        throw new ArgumentNullException();
                    case 1:
                        return found.Single();
                    default:
                        throw new ArgumentException();
                }
            }
        }
    }
}