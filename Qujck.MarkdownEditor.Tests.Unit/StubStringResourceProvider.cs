using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qujck.MarkdownEditor.Infrastructure;

namespace Qujck.MarkdownEditor.Tests.Unit
{
    public class StubStringResourceProvider : IStringResourceProvider
    {
        public string Many(params string[] prefixes)
        {
            return string.Join(Environment.NewLine, prefixes);
        }

        public string Single(params string[] names)
        {
            return string.Join(Environment.NewLine, names);
        }
    }
}
