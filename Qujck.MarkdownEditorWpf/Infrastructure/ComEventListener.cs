using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Qujck.MarkdownEditor.Infrastructure
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    public class ComEventListener
    {
        private readonly Action action;

        public ComEventListener(Action action)
        {
            this.action = action;
        }

        [DispId(0)]
        public void NameDoesNotMatter(object data)
        {
            this.action();
        }
    }
}
