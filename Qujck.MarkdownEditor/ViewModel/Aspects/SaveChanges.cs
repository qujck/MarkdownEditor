using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Qujck.MarkdownEditor.Infrastructure;
using Qujck.MarkdownEditor.ViewModel.Commands;
using Qujck.MarkdownEditor.ViewModel.Queries;

namespace Qujck.MarkdownEditor.ViewModel.Aspects
{
    internal sealed class SaveChangesHandler<TViewModelParameter> : IViewModelCommand<TViewModelParameter>
        where TViewModelParameter : ViewModelParameter
    {
        private readonly IViewModelCommand<TViewModelParameter> decorated;
        private readonly IViewModelQuery<CanSaveFile> canSaveFile;
        private readonly IViewModelCommand<SaveFile> saveFile;

        public SaveChangesHandler(
            IViewModelCommand<TViewModelParameter> decorated,
            IViewModelQuery<CanSaveFile> canSaveFile,
            IViewModelCommand<SaveFile> saveFile)
        {
            this.decorated = decorated;
            this.canSaveFile = canSaveFile;
            this.saveFile = saveFile;
        }

        public void Run(TViewModelParameter viewModelParameter)
        {
            bool @continue = true;
            if (this.CanSave(viewModelParameter.DynamicViewModel as DocumentViewModel))
            {
                var result = MessageBox.Show(
                    "Do you want to save changes?", 
                    "Save changes", 
                    MessageBoxButton.YesNoCancel, 
                    MessageBoxImage.Exclamation);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        this.Save(viewModelParameter.DynamicViewModel as DocumentViewModel);
                        if (this.CanSave(viewModelParameter.DynamicViewModel as DocumentViewModel))
                        {
                            @continue = false;
                        }
                        break;
                    case MessageBoxResult.No:
                        break;
                    default:
                        @continue = false;
                        break;
                }
            }

            if (@continue)
            {
                this.decorated.Run((dynamic)viewModelParameter);
            }
        }

        private bool CanSave(DocumentViewModel viewModel)
        {
            return this.canSaveFile.Execute(new CanSaveFile(viewModel));
        }

        private void Save(DocumentViewModel viewModel)
        {
            this.saveFile.Run(new SaveFile(viewModel));
        }
    }
}
