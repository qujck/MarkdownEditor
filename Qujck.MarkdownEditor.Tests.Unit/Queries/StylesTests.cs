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
    public class StylesTests
    {
        string[] resources = new string[] { Constants.Content.Bootstrap + Environment.NewLine + Constants.Content.SiteCss };

        [Fact]
        public void Execute_Always_ReturnsExpectedStringResource()
        {
            var handler = this.HandlerFactory();

            var result = handler.Execute();

            result.Should().Contain(string.Join(Environment.NewLine, resources));
        }

        private Query.Handlers.StylesHandler HandlerFactory()
        {
            return new Query.Handlers.StylesHandler(
                new StubStringResourceProvider());
        }
    }
}
