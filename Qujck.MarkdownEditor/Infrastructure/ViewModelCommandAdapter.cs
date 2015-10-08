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
        private readonly string canExecuteMethodName;
        private readonly string executeMethodName;

        public ViewModelCommandAdapter(string executeMethodName)
        {
            this.executeMethodName = executeMethodName;
            this.canExecuteMethodName = null;
        }

        public ViewModelCommandAdapter(string executeMethodName, string canExecuteMethodName)
        {
            this.executeMethodName = executeMethodName;
            this.canExecuteMethodName = canExecuteMethodName;
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

        private Func<bool> CanExecuteMethod
        {
            get
            {
                return this.DataContext[this.canExecuteMethodName] as Func<bool>;
            }
        }

        private Action ExecuteMethod
        {
            get
            {
                return this.DataContext[this.executeMethodName] as Action;
            }
        }

        event EventHandler ICommand.CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        bool ICommand.CanExecute(object parameter)
        {
            return this.canExecuteMethodName == null
                ? true
                : this.CanExecuteMethod();
        }

        void ICommand.Execute(object parameter)
        {
            this.ExecuteMethod();
        }
    }
}
