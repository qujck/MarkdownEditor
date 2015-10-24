using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using NUnit.Framework;
using FluentAssertions;
using ICSharpCode.AvalonEdit;
using Qujck.MarkdownEditor.ViewModel;
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
            body.innerHTML.Should().Be("RenderedViewInitialiserTests");
        }

        [Test]
        [STAThread]
        public void DocumentView_Always_SetsHtmlIsLoaded()
        {
            var documentView = this.DocumentViewFactory();
            var behaviour = this.FindBehaviour(documentView);
            behaviour.RenderedView_LoadCompleted(documentView, null);

            var viewModel = (DocumentViewModel)documentView.DataContext;
            viewModel.HtmlIsLoaded.Should().BeTrue();
        }

        private mshtml.IHTMLElement Body(DocumentView documentView)
        {
            mshtml.IHTMLElement body = null;
            while (body == null || body.innerHTML == null)
            {
                this.Pause();
                var document = (mshtml.IHTMLDocument2)documentView.RenderedView.Document;
                body = (mshtml.IHTMLElement)document.body;
            }
            return body;
        }

        private void Pause()
        {
            Thread.Sleep(10);
            DispatcherUtil.DoEvents();
        }

        private DocumentView DocumentViewFactory()
        {
            var documentView = new DocumentView();
            var behaviour = this.FindBehaviour(documentView);
            behaviour.HtmlQueryHandler = new StubQueryHandler<Query.Html>(() => "<!DOCTYPE html><html><body>RenderedViewInitialiserTests</body></html>");

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
