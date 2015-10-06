using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qujck.MarkdownEditor
{
    public interface IDependencyResolver
    {
        T Resolve<T>() where T : class;
    }
}
