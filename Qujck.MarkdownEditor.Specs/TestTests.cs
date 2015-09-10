using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xbehave;
using FluentAssertions;
using Qujck.MarkdownEditor.Infrastructure;
using Qujck.MarkdownEditor.Queries;

namespace Qujck.MarkdownEditor.Specs
{
    public class TestTests
    {
        const string TestResponse = "HtmlTests_Layout";

        [Scenario]
        public void Execute_Always_ReturnsExpectedStringResource1(
            Query.Handlers.HtmlHandler handler,
            string request,
            string response)
        {
            "Given I have a HtmlHandler"
                .f(() => handler = new Query.Handlers.HtmlHandler(
                    new StubStringResourceProvider()));

            "When I call the Execute method"
                .f(() => response = handler.Execute());

            "Then the HtmlHandler requests the Layout resource"
                .f(() => request = Constants.Content.Layout);

            "And the HtmlHandler returns the Layout resource"
                .f(() => response.Should().Be(Constants.Content.Layout));
        }

        private class StubStringResourceProvider : IStringResourceProvider
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
}