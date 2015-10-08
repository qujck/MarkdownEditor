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
                if (rootProvider != null)
                {
                    this.frameworkElement = rootProvider.RootObject as FrameworkElement;
                }
            }

            return this;
        }

        private AbstractViewModel ViewModel
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
            if (parameter != null)
            {
                throw new ArgumentException();
            }

            return this.canExecuteMethodName == null
                ? true
                : this.ViewModel.CanExecute(this.canExecuteMethodName);
        }

        void ICommand.Execute(object parameter)
        {
            if (parameter != null)
            {
                throw new ArgumentException();
            }

            this.ViewModel.Execute(this.executeMethodName);
        }
    }
}
