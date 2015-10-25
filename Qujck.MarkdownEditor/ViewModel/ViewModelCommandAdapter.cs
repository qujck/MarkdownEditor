using System;
using System.Reflection;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Input;
using System.Xaml;
using Qujck.MarkdownEditor.ViewModel.Core;
using Qujck.MarkdownEditor.Infrastructure;

namespace Qujck.MarkdownEditor.ViewModel
{
    [MarkupExtensionReturnType(typeof(ICommand))]
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

        private DynamicViewModel ViewModel
        {
            get
            {
                if (this.frameworkElement.DataContext == null ||
                    !typeof(DynamicViewModel).IsAssignableFrom(this.frameworkElement.DataContext.GetType()))
                {
                    throw new InvalidProgramException();
                }

                return this.frameworkElement.DataContext as DynamicViewModel;
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
                : BootStrapper.ExecuteViewQuery(this.canExecuteMethodName, this.ViewModel);
        }

        void ICommand.Execute(object parameter)
        {
            if (parameter != null)
            {
                throw new ArgumentException();
            }

            BootStrapper.ExecuteViewCommand(this.executeMethodName, this.ViewModel);
        }
    }
}
