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
    public class HtmlTests
    {
        const string TestResponse = "HtmlTests_Layout";

        [Fact]
        public void Execute_Always_ReturnsExpectedStringResource()
        {
            var handler = this.HandlerFactory();

            var result = handler.Execute();

            result.Should().Be(TestResponse);
        }

        private Query.Handlers.HtmlHandler HandlerFactory()
        {
            return new Query.Handlers.HtmlHandler(
                new StubStringResourceProvider(request =>
                {
                    return request == Constants.Content.Layout
                        ? TestResponse
                        : null;
                }));
        }
    }
}
