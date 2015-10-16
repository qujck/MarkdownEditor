﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using Qujck.MarkdownEditor.Infrastructure;

namespace Qujck.MarkdownEditor.ViewModel.Commands
{
    internal sealed class SaveFile : ViewModelParameter
    {
        public SaveFile(DocumentViewModel viewModel) : base(viewModel)
        {
        }
    }

    internal sealed class SaveFileHandler : IViewModelCommand<SaveFile>
    {
        public void Run(SaveFile viewModelParameter)
        {
            string filePath = (string)viewModelParameter.ViewModel[Constants.DocumentViewModel.FilePath];
            string fileName = Path.GetFileName(filePath);

            if (string.IsNullOrEmpty(fileName))
            {
                var dialog = new SaveFileDialog()
                {
                    FileName = fileName,
                    Filter = "Markdown Files(*.md)|*.md|All(*.*)|*"
                };

                if (dialog.ShowDialog() == true)
                {
                    filePath = dialog.FileName;
                }
            }

            if (!string.IsNullOrEmpty(filePath))
            { 
                File.WriteAllText(
                    filePath, 
                    (string)viewModelParameter.ViewModel[Constants.DocumentViewModel.CurrentText]);

                viewModelParameter.ViewModel[Constants.DocumentViewModel.OpeningText] = 
                    viewModelParameter.ViewModel[Constants.DocumentViewModel.CurrentText];

                viewModelParameter.ViewModel[Constants.DocumentViewModel.FilePath] = filePath;
            }
        }
    }
}
