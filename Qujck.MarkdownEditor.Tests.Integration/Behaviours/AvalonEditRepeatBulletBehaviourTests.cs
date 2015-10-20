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
        [TestCase, STAThread]
        public void TextEditor_WhenEnteringWithoutBullets_DoesNothing()
        {
            string entered = "not a bullet" + Environment.NewLine;
            string expected = entered;

            var documentView = this.RunTest(entered);

            documentView.TextEditor.Text.Should().Be(expected);
        }

        [TestCase, STAThread]
        public void TextEditor_WhenEnteringABulletedList_PrefixesTheNextLineWithTheBullet(
            [Values("-", "+", "*")]string bullet)
        {
            string entered = bullet + " bullet" + Environment.NewLine;
            string expected = entered + bullet + " ";

            var documentView = this.RunTest(entered);

            documentView.TextEditor.Text.Should().Be(expected);
        }

        [TestCase, STAThread]
        public void TextEditor_WhenEnteringANumberedList_PrefixesTheNextLineWithTheNextNumber(
            [Values(1, 2)]int number)
        {
            string entered = number.ToString() + ". bullet" + Environment.NewLine;
            string expected = entered + (number + 1).ToString() + ". ";

            var documentView = this.RunTest(entered);

            documentView.TextEditor.Text.Should().Be(expected);
        }

        [TestCase, STAThread]
        public void TextEditor_WhenEnteringAfterAnEmptyBullet_RemovesTheEmptyBullet(
            [Values("-", "+", "*")]string bullet)
        {
            string entered = bullet + " " + Environment.NewLine + "any text";
            string expected = Environment.NewLine + "any text";

            var documentView = this.RunTest(entered);

            documentView.TextEditor.Text.Should().Be(expected);
        }

        private DocumentView RunTest(string startText)
        {
            var documentView = new DocumentView();

            var behaviour = (AvalonEditRepeatBulletBehaviour)(
                from b in Interaction.GetBehaviors(documentView)
                where b.GetType() == typeof(AvalonEditRepeatBulletBehaviour)
                select b).Single();

            documentView.TextEditor.Document.Text = startText;
            behaviour.LineInsertedCallBack(
                documentView.TextEditor.Document.Lines[0],
                documentView.TextEditor.Document.Lines[1]);

            behaviour.TextEditor_TextChanged(documentView.TextEditor, null);

            return documentView;
        }
    }
}
