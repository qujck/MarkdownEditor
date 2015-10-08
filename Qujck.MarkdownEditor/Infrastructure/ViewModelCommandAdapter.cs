using System;
using System.Reflection;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Input;
using System.Xaml;

namespace Qujck.MarkdownEditor.Infrastructure
{
    public class ViewModelCommandAdapter : MarkupExtension, ICommand
    {
        private FrameworkElement frameworkElement;

        private readonly string canExecute;

        private readonly string executed;

        public ViewModelCommandAdapter(string executed)
        {
            this.canExecute = null;
            this.executed = executed;
        }

        public ViewModelCommandAdapter(string executed, string canExecute)
        {
            this.executed = executed;
            this.canExecute = canExecute;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (this.frameworkElement == null)
            {
                var rootProvider = (IRootObjectProvider)serviceProvider
                           .GetService(typeof(IRootObjectProvider));
                this.frameworkElement = rootProvider.RootObject as FrameworkElement;
            }

            return this;
        }

        private AbstractViewModel DataContext
        {
            get
            {
                if (this.frameworkElement.DataContext == null ||
                    !typeof(AbstractViewModel).IsAssignableFrom(this.frameworkElement.DataContext.GetType()))
                {
                    throw new InvalidProgramException();
                }

                return this.frameworkElement.DataContext as AbstractViewModel;
            }
        }

        event EventHandler ICommand.CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        bool ICommand.CanExecute(object parameter)
        {
            return this.canExecute == null
                ? true
                : (this.DataContext[this.canExecute] as Func<bool>)();
        }

        void ICommand.Execute(object parameter)
        {
            (this.DataContext[this.executed] as Action)();
        }
    }
}
