using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qujck.MarkdownEditor.Infrastructure;

namespace Qujck.MarkdownEditor.ViewModel
{
    internal abstract class ViewModelParameter : IViewModelParameter
    {
        private DynamicViewModel viewModel { get; set; }

        public ViewModelParameter(DynamicViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public object this[string key]
        {
            get
            {
                return this.viewModel[key];
            }

            set
            {
                this.viewModel[key] = value;
            }
        }

        public DynamicViewModel DynamicViewModel
        {
            get
            {
                return this.viewModel;
            }
        }
    }
}
