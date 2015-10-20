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
using Qujck.MarkdownEditor.ViewModel;
using Qujck.MarkdownEditor.Behaviours;
using System.Windows.Threading;

namespace Qujck.MarkdownEditor.Tests.Integration.Behaviours
{
    public class AvalonEditRepeatBulletBehaviourTests
    {
        [TestCase]
        [STAThread]
        public void TextEditor_WhenEnteringWithoutBullets_DoesNothing()
        {
            string entered = "not a bullet";
            string expected = entered + Environment.NewLine;

            var documentView = this.SetTextWithNewLine(entered);

            documentView.TextEditor.Text.Should().Be(expected);
        }

        [TestCase("*")]
        [TestCase("+")]
        [TestCase("-")]
        [STAThread]
        public void TextEditor_NewLineAfterBulletedListItem_PrefixesTheNextLineWithTheBullet(string bullet)
        {
            string entered = bullet + " bullet";
            string expected = entered + Environment.NewLine + bullet + " ";

            var documentView = this.SetTextWithNewLine(entered);

            documentView.TextEditor.Text.Should().Be(expected);
        }

        [TestCase(1)]
        [TestCase(2)]
        [STAThread]
        public void TextEditor_NewLineAfterNumberedListItem_PrefixesTheNextLineWithTheNextNumber(int number)
        {
            string entered = number.ToString() + ". bullet";
            string expected = entered + Environment.NewLine + (number + 1).ToString() + ". ";

            var documentView = this.SetTextWithNewLine(entered);

            documentView.TextEditor.Text.Should().Be(expected);
        }

        [TestCase("*")]
        [TestCase("+")]
        [TestCase("-")]
        [STAThread]
        public void TextEditor_NewLineAfterAnEmptyBullet_RemovesEmptyBullet(string bullet)
        {
            string entered = bullet + " ";
            string expected = Environment.NewLine;

            var documentView = this.SetTextWithNewLine(entered);

            documentView.TextEditor.Text.Should().Be(expected);
        }

        [TestCase]
        [STAThread]
        public void TextEditor_Only_ProcessesBulletsWithNewLineAtEndOnce()
        {
            string entered = "any text" + Environment.NewLine + "- any text";
            string additional = "more text";
            string expected = entered + Environment.NewLine + "- " + additional;

            var documentView = this.SetTextWithNewLine(entered, 1);
            documentView = this.AppendText(documentView, additional);

            documentView.TextEditor.Text.Should().Be(expected);
        }

        private DocumentView SetTextWithNewLine(string startText, int currentLine = 0)
        {
            var documentView = new DocumentView();
            var behaviour = this.FindBehaviour(documentView);

            documentView.TextEditor.Document.Text += startText + Environment.NewLine;

            behaviour.LineInsertedCallBack(
                documentView.TextEditor.Document.Lines[currentLine],
                documentView.TextEditor.Document.Lines[currentLine + 1]);

            behaviour.TextEditor_TextChanged(documentView.TextEditor, null);

            return documentView;
        }

        private DocumentView AppendText(DocumentView documentView, string extraText)
        {
            var behaviour = this.FindBehaviour(documentView);

            documentView.TextEditor.Document.Text += extraText;

            behaviour.TextEditor_TextChanged(documentView.TextEditor, null);

            return documentView;
        }

        private AvalonEditRepeatBulletBehaviour FindBehaviour(DocumentView documentView)
        {
            return Interaction.GetBehaviors(documentView)
                .OfType<AvalonEditRepeatBulletBehaviour>()
                .Single();
        }
    }
}
