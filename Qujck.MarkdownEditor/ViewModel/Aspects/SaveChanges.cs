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
    internal sealed class SaveChanges : ViewModelParameter
    {
        public SaveChanges(DocumentViewModel viewModel) : base(viewModel)
        {
        }
    }

    internal sealed class SaveChangesHandler : IViewModelCommand<ViewModelParameter>
    {
        private readonly dynamic decorated;
        private readonly IViewModelQuery<CanSaveFile> canSaveFile;
        private readonly IViewModelCommand<SaveFile> saveFile;

        public SaveChangesHandler(
            dynamic decorated,
            IViewModelQuery<CanSaveFile> canSaveFile,
            IViewModelCommand<SaveFile> saveFile)
        {
            this.decorated = decorated;
            this.canSaveFile = canSaveFile;
            this.saveFile = saveFile;
        }

        public void Run(ViewModelParameter viewModelParameter)
        {
            if (this.CanSave(viewModelParameter.ViewModel as DocumentViewModel))
            {
                var result = MessageBox.Show(
                    "Do you want to save changes?", 
                    "Save changes", 
                    MessageBoxButton.YesNoCancel, 
                    MessageBoxImage.Exclamation);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        this.Save(viewModelParameter.ViewModel as DocumentViewModel);
                        if (!this.CanSave(viewModelParameter.ViewModel as DocumentViewModel))
                        {
                            this.decorated.Run((dynamic)viewModelParameter);
                        }
                        break;
                    case MessageBoxResult.No:
                        this.decorated.Run((dynamic)viewModelParameter);
                        break;
                    default:
                        break;
                }
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
