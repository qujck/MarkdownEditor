using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qujck.MarkdownEditor.Infrastructure;

namespace Qujck.MarkdownEditor.Tests.Integration
{
    internal class StubStringResourceProvider : IStringResourceProvider
    {
        private readonly string result;

        public StubStringResourceProvider(string result)
        {
            this.result = result;
        }

        public string Many(params string[] prefixes)
        {
            return result;
        }

        public string One(params string[] names)
        {
            return result;
        }
    }
}
