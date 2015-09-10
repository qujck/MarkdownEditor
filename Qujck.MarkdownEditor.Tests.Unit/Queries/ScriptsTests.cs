using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Qujck.MarkdownEditor.Infrastructure;
using Qujck.MarkdownEditor.Queries;

namespace Qujck.MarkdownEditor.Tests.Unit.Queries
{
    public class ScriptsTests
    {
        string[] resources = new string[] 
        {
            Constants.Scripts.Marked
        };

        [Fact]
        public void Execute_Always_ReturnsExpectedStringResource()
        {
            var handler = this.HandlerFactory();

            var result = handler.Execute();

            result.Should().Be(string.Join(Environment.NewLine, resources));
        }

        private Query.Handlers.ScriptsHandler HandlerFactory()
        {
            return new Query.Handlers.ScriptsHandler(
                new StubStringResourceProvider());
        }
    }
}
