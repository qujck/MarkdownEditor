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
using Qujck.MarkdownEditor.ViewModel.Aspects;
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
            return ((dynamic)handler.Item2).Execute(parameter);
        }

        internal static void ExecuteViewCommand(string name, DynamicViewModel viewModel)
        {
            var handler = Resolver.ResolveViewModelCommand(name);
            dynamic parameter = Activator.CreateInstance(
                handler.Item1,
                viewModel);
            ((dynamic)handler.Item2).Run(parameter);
        }

        internal sealed class DependencyResolver
        {
            private ICommandHandler<Command.RenderMarkdown> renderMarkdownHandler;
            private IQueryHandler<Query.Html, string> htmlQueryHandler;
            private IQueryHandler<Query.Scripts, string> scriptsQueryHandler;
            private IQueryHandler<Query.Styles, string> stylesQueryHandler;
            private IStringResourceProvider stringResourceProvider;
            private IViewModelQuery<CanSaveFile> canSaveFileHandler;
            private IViewModelCommand<ViewModelParameter> newFileHandler;
            private NextViewHandler nextViewHandler;
            private IViewModelCommand<ViewModelParameter> openFileHandler;
            private PreviousViewHandler previousViewHandler;
            private SaveFileHandler saveFileHandler;

            private DependencyResolver()
            {
            }

            internal static DependencyResolver Build()
            {
                return new DependencyResolver()
                    .RegisterProviders()
                    .RegisterCommandHandlers()
                    .RegisterQueryHandlers()
                    .RegisterViewModelQueries()
                    .RegisterViewModelCommands();
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

            private DependencyResolver RegisterViewModelQueries()
            {
                this.canSaveFileHandler = new CanSaveFileHandler();

                return this;
            }

            private DependencyResolver RegisterViewModelCommands()
            {
                this.saveFileHandler = new SaveFileHandler();
                this.newFileHandler = new SaveChangesHandler(new NewFileHandler(), this.canSaveFileHandler, saveFileHandler);
                this.nextViewHandler = new NextViewHandler();
                this.openFileHandler = new SaveChangesHandler(new OpenFileHandler(), this.canSaveFileHandler, saveFileHandler);
                this.previousViewHandler = new PreviousViewHandler();

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
                if (name == "NewFile")
                    return new Tuple<Type, object>(typeof(NewFile), this.newFileHandler);
                else if (name == "NextView")
                    return new Tuple<Type, object>(typeof(NextView), this.nextViewHandler);
                else if (name == "OpenFile")
                    return new Tuple<Type, object>(typeof(OpenFile), this.openFileHandler);
                else if (name == "PreviousView")
                    return new Tuple<Type, object>(typeof(PreviousView), this.previousViewHandler);
                else if (name == "SaveFile")
                    return new Tuple<Type, object>(typeof(SaveFile), this.saveFileHandler);
                else
                    throw new ArgumentNullException();
            }

            internal Tuple<Type, object> ResolveViewModelQuery(string name)
            {
                return new Tuple<Type, object>(typeof(CanSaveFile), this.canSaveFileHandler);
            }
        }
    }
}