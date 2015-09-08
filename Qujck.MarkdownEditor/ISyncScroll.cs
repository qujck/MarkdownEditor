using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qujck.MarkdownEditor
{
    public interface ISyncScroll
    {
        void Scroll(double pos);
        bool IsHandleCreated { get; }
        bool IsScrolling { get; }
    }
}
