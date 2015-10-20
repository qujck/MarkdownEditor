using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using NUnit.Framework;
using FluentAssertions;
using ICSharpCode.AvalonEdit;
using Qujck.MarkdownEditor.Queries;
using Qujck.MarkdownEditor.Behaviours;
using System.Windows.Threading;

namespace Qujck.MarkdownEditor.Tests.Integration.Behaviours
{
    public class RenderedViewInitialiserTests
    {
        [Test]
        [STAThread]
        public void DocumentView_Always_LoadsHtml()
        {
            var documentView = this.DocumentViewFactory();
            var behaviour = this.FindBehaviour(documentView);

            behaviour.LoadHtml();
            var body = this.Body(documentView);

            body.Should().Be("RenderedViewInitialiserTests");
        }

        private mshtml.IHTMLElement2 Body(DocumentView documentView)
        {
            var document = (mshtml.IHTMLDocument2)documentView.RenderedView.Document;
            var body = (mshtml.IHTMLElement2)document.body;

            return body;
        }

        private DocumentView DocumentViewFactory()
        {
            var documentView = new DocumentView();
            var behaviour = this.FindBehaviour(documentView);
            behaviour.HtmlQueryHandler = new StubQueryHandler<Query.Html, string>(() => "<html><body>RenderedViewInitialiserTests</body></html>");

            return documentView;
        }

        private RenderedViewInitialiser FindBehaviour(DocumentView documentView)
        {
            return Interaction.GetBehaviors(documentView)
                .OfType<RenderedViewInitialiser>()
                .Single();
        }
    }
}
