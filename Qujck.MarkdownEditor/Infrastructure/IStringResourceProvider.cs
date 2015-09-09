using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qujck.MarkdownEditor.Infrastructure
{
    public interface IStringResourceProvider
    {
        string Single(string name);

        string Many(string name);
    }
}
