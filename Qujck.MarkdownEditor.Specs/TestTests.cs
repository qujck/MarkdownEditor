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
                    new StubStringResourceProvider(name =>
                    {
                        request = name;
                        return TestResponse;
                    })));

            "When I call the Execute method"
                .f(() => response = handler.Execute());

            "Then the HtmlHandler requests the Layout resource"
                .f(() => request.Should().Be(Constants.Content.Layout));

            "And the HtmlHandler returns the Layout resource"
                .f(() => response.Should().Be(TestResponse));
        }

        private class StubStringResourceProvider : IStringResourceProvider
        {
            private readonly Func<string, string> response;

            public StubStringResourceProvider(Func<string, string> response)
            {
                this.response = response;
            }

            public string Name { get; private set; }

            public string Many(string name)
            {
                throw new NotImplementedException();
            }

            public string Single(string name)
            {
                this.Name = name;
                return this.response(name);
            }
        }
    }
}
