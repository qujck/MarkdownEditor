using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Qujck.MarkdownEditor.Infrastructure
{
    public interface IStringResourceProvider
    {
        string Single(params string[] names);

        string Many(params string[] prefixes);
    }
}
