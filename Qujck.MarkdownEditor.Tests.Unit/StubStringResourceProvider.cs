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
        private readonly Func<string, string> response;

        public StubStringResourceProvider(string response) :
            this((request) => response)
        {
        }

        public StubStringResourceProvider(Func<string, string> response)
        {
            this.response = response;
        }

        public string Name { get; private set; }

        public string Many(string name)
        {
            this.Name = name;
            return this.response(name);
        }

        public string Single(string name)
        {
            this.Name = name;
            return this.response(name);
        }
    }
}
