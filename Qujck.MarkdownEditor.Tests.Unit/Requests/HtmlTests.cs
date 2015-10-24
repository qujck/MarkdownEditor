using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Qujck.MarkdownEditor.Infrastructure;
using Qujck.MarkdownEditor.Requests;

namespace Qujck.MarkdownEditor.Tests.Unit.Requests
{
    public class HtmlTests
    {
        [Fact]
        public void Execute_Always_ReturnsExpectedStringResource()
        {
            var handler = this.HandlerFactory();

            var result = handler.Execute();

            result.Should().Be(Constants.Content.Layout);
        }

        private Strings.Handlers.HtmlHandler HandlerFactory()
        {
            return new Strings.Handlers.HtmlHandler(
                new StubNamedResourcesProvider());
        }
    }
}
