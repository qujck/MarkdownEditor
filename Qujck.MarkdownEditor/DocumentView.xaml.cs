using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;
using Qujck.MarkdownEditor.ViewModel;
using Qujck.MarkdownEditor.Requests;

namespace Qujck.MarkdownEditor
{
    public partial class DocumentView : UserControl
    {
        public DocumentView()
        {
            InitializeComponent();
            this.TextEditor.Focus();
        }

        private void DataModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Constants.DocumentViewModel.OpeningText)
            {
                var model = sender as DocumentViewModel;

                this.TextEditor.Text = (string)model[Constants.DocumentViewModel.OpeningText];
            }
            else if (e.PropertyName == Constants.DocumentViewModel.CurrentText)
            {
                var model = sender as DocumentViewModel;
                string currentText = (string)model[Constants.DocumentViewModel.CurrentText];

                if (this.TextEditor.Text != currentText)
                {
                    this.TextEditor.Text = currentText;
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            string fileName = (string)Application.Current.Properties["OpenFile"];
            if (fileName != null)
            {
                var timer = new DispatcherTimer(DispatcherPriority.Background);
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += ((x, y) =>
                {
                    timer.Stop();
                    timer = null;
                    var viewModelParameter = (DocumentViewModel)((DocumentView)e.Source).DataContext;
                    Infrastructure.BootStrapper
                        .Resolver
                        .Resolve<IActionRequestHandler<Actions.LoadWholeFile>>()
                        .Run(viewModelParameter, fileName);
                });
                timer.Start();
            }
        }
    }
}
