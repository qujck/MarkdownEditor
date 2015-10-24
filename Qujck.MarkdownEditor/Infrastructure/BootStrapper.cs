using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using Qujck.MarkdownEditor.Infrastructure;
using Qujck.MarkdownEditor.Commands;
using Qujck.MarkdownEditor.Requests;
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
            private ICommandRequestHandler<Command.RenderMarkdown> renderMarkdownHandler;
            private IStringRequestHandler<Strings.NamedResources> namedResourcesHandler;
            private IStringRequestHandler<Strings.PrefixedResources> prefixedResourcesHandler;
            private IStringRequestHandler<Strings.Html> htmlHandler;
            private IStringRequestHandler<Strings.Scripts> scriptsHandler;
            private IStringRequestHandler<Strings.Styles> stylesHandler;
            private IViewModelQuery<CanSaveFile> canSaveFileHandler;
            private IViewModelCommand<NewFile> newFileHandler;
            private IViewModelCommand<NextView> nextViewHandler;
            private IViewModelCommand<OpenFile> openFileHandler;
            private IViewModelCommand<PreviousView> previousViewHandler;
            private IViewModelCommand<SaveFile> saveFileHandler;
            private IViewModelCommand<Shutdown> shutdownHandler;

            private DependencyResolver()
            {
            }

            internal static DependencyResolver Build()
            {
                return new DependencyResolver()
                    .RegisterCommandHandlers()
                    .RegisterQueryHandlers()
                    .RegisterViewModelQueries()
                    .RegisterViewModelCommands();
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
                this.namedResourcesHandler = new Strings.Handlers.NamedResourcesHandler();
                this.prefixedResourcesHandler = new Strings.Handlers.PrefixedResourcesHandler();

                this.scriptsHandler = new PrettifyScripts(
                    new Strings.Handlers.ScriptsHandler(
                        this.namedResourcesHandler),
                    this.namedResourcesHandler,
                    this.prefixedResourcesHandler);

                this.stylesHandler = new PrettifyStyles(
                    new Strings.Handlers.StylesHandler(
                        this.namedResourcesHandler),
                    this.namedResourcesHandler);

                this.htmlHandler = new PrepareHtml(
                    new Strings.Handlers.HtmlHandler(
                        this.namedResourcesHandler),
                    this.stylesHandler,
                    this.scriptsHandler);

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
                this.newFileHandler = new SaveChangesHandler<NewFile>(new NewFileHandler(), this.canSaveFileHandler, saveFileHandler);
                this.nextViewHandler = new NextViewHandler();
                this.openFileHandler = new SaveChangesHandler<OpenFile>(new OpenFileHandler(), this.canSaveFileHandler, saveFileHandler);
                this.previousViewHandler = new PreviousViewHandler();
                this.shutdownHandler = new ShutdownHandler();

                return this;
            }

            internal T Resolve<T>() where T : class
            {
                return this.Resolve(typeof(T)) as T;
            }

            internal object Resolve(Type serviceType)
            {
                if (serviceType == typeof(IStringRequestHandler<Strings.NamedResources>))
                {
                    return this.namedResourcesHandler;
                }
                else if (serviceType == typeof(IStringRequestHandler<Strings.Html>))
                {
                    return this.htmlHandler;
                }
                else if (serviceType == typeof(ICommandRequestHandler<Command.RenderMarkdown>))
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
                else if (name == "Shutdown")
                    return new Tuple<Type, object>(typeof(Shutdown), this.shutdownHandler);
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