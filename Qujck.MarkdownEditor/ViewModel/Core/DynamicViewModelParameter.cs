using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qujck.MarkdownEditor.ViewModel.Core
{
    internal abstract class DynamicViewModelParameter : IViewModelParameter
    {
        private DynamicViewModel viewModel { get; set; }

        public DynamicViewModelParameter(DynamicViewModel viewModel)
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
