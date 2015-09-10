﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xbehave;
using FluentAssertions;
using Qujck.MarkdownEditor.Infrastructure;
using Qujck.MarkdownEditor.Queries;

namespace Qujck.MarkdownEditor.Tests.Unit.Queries
{
    public class HtmlTests
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
