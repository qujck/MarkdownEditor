using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interactivity;
using NUnit.Framework;
using FluentAssertions;
using ICSharpCode.AvalonEdit;
using Qujck.MarkdownEditor.ViewModel;
using Qujck.MarkdownEditor.Behaviours;

namespace Qujck.MarkdownEditor.Tests.Integration.Behaviours
{
    public class AvalonEditInitialiserTests
    {
        const string xshd = @"<?xml version='1.0'?>
<SyntaxDefinition name = 'MarkDown' extensions='.md' xmlns='http://icsharpcode.net/sharpdevelop/syntaxdefinition/2008'>
<Color name='Heading1'/>
<RuleSet/>
</SyntaxDefinition>";

        [TestCase]
        [STAThread]
        public void TextEditor_Always_LoadsCustomSyntaxHighlighter()
        {
            var documentView = this.DocumentViewFactory();
            var behaviour = this.FindBehaviour(documentView);

            var before = documentView.TextEditor.SyntaxHighlighting;
            behaviour.LoadHighlighter();
            var after = documentView.TextEditor.SyntaxHighlighting;

            before.Should().BeNull();
            after.Should().NotBeNull();
        }

        private DocumentView DocumentViewFactory()
        {
            var documentView = new DocumentView();
            var behaviour = this.FindBehaviour(documentView);
            behaviour.NamedResources = new StubNamedResourcesProvider(xshd);

            return documentView;
        }

        private AvalonEditInitialiser FindBehaviour(DocumentView documentView)
        {
            return Interaction.GetBehaviors(documentView)
                .OfType<AvalonEditInitialiser>()
                .Single();
        }
    }
}
