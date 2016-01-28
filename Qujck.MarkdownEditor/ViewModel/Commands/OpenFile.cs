using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using Qujck.MarkdownEditor.ViewModel.Core;
using System.Windows;

namespace Qujck.MarkdownEditor.ViewModel.Commands
{
    internal sealed class OpenFile : DynamicViewModelParameter
    {
        public OpenFile(DocumentViewModel viewModel) : base(viewModel)
        {
        }

        public string ApplicationFileName
        {
            get
            {
                string fileName = (string)Application.Current.Properties["OpenFile"];
                if (fileName != null)
                {
                    Application.Current.Properties.Remove("OpenFile");
                }
                return fileName;
            }
        }
    }

    internal sealed class OpenFileHandler : IViewModelCommand<OpenFile>
    {
        public void Run(OpenFile viewModelParameter)
        {
            string fileName;
            string applicationFileName = viewModelParameter.ApplicationFileName;
            if (applicationFileName != null)
            {
                fileName = applicationFileName;
            }
            else
            {
                fileName = this.GetFileName();
            }
            string text = File.ReadAllText(fileName);
            if (fileName != null)
            {
                viewModelParameter[Constants.DocumentViewModel.FilePath] = fileName;
                viewModelParameter[Constants.DocumentViewModel.OpeningText] = text;
                viewModelParameter[Constants.DocumentViewModel.CurrentText] = text;
            }
        }

        public string GetFileName()
        {
            var dialog = new OpenFileDialog()
            {
                Filter = "Markdown Files(*.md)|*.md|All(*.*)|*"
            };

            if (dialog.ShowDialog() == true)
            {
                return dialog.FileName;
            }
            else
            {
                return null;
            }
        }
    }
}
