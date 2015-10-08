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

namespace Qujck.MarkdownEditor
{
    public partial class DocumentView : UserControl
    {
        public bool HtmlIsLoaded;

        public DocumentView()
        {
            InitializeComponent();
        }

        private void DataModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == DocumentViewModel.OpeningText)
            {
                var model = sender as DocumentViewModel;

                this.TextEditor.Text = model[DocumentViewModel.OpeningText] as string;
            }
        }
    }
}
