using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using Qujck.MarkdownEditor.Infrastructure;

namespace Qujck.MarkdownEditor.Behaviours
{
    public sealed class AvalonEditRepeatBulletBehaviour : Behavior<DocumentView>
    {
        const string Continue = @"^(\- |\* |\+ |\d+\. )";
        const string End = @"^(\- |\* |\+ |\d+\. )$";
        const string Number = @"^\d+";
        const string NextNumber = @"{0}. ";
        private readonly NextBulletLineTracker tracker;
        private ISegment currentLine;
        private ISegment newLine;
        private bool processing;

        public AvalonEditRepeatBulletBehaviour()
        {
            this.tracker = new NextBulletLineTracker(LineInsertedCallBack);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            base.AssociatedObject.Loaded += AssociatedObject_Loaded;
            base.AssociatedObject.Unloaded += AssociatedObject_Unloaded;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            base.AssociatedObject.Loaded -= AssociatedObject_Loaded;
            base.AssociatedObject.Unloaded -= AssociatedObject_Unloaded;
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            this.AssociatedObject.TextEditor.Document.LineTrackers.Add(this.tracker);
            this.AssociatedObject.TextEditor.TextChanged += TextEditor_TextChanged;
        }

        private void AssociatedObject_Unloaded(object sender, RoutedEventArgs e)
        {
            this.AssociatedObject.TextEditor.Document.LineTrackers.Remove(this.tracker);
            this.AssociatedObject.TextEditor.TextChanged -= TextEditor_TextChanged;
        }

        public void LineInsertedCallBack(ISegment currentLine, ISegment newLine)
        {
            this.currentLine = currentLine;
            this.newLine = newLine;
        }

        public void TextEditor_TextChanged(object sender, EventArgs e)
        {
            var textEditor = (TextEditor)sender;
            if (!this.processing)
            {
                if (this.currentLine != null)
                {
                    string text = textEditor.Document.GetText(this.currentLine);
                    Match match;

                    if ((match = Regex.Match(text, End)).Success)
                    {
                        this.processing = true;
                        textEditor.Document.Remove(this.currentLine);
                    }
                    else if ((match = Regex.Match(text, Continue)).Success)
                    {
                        this.processing = true;
                        var findNumber = Regex.Match(match.Value, Number);
                        if (findNumber.Success)
                        {
                            int i = int.Parse(findNumber.Value);
                            textEditor.Document.Insert(this.newLine.Offset, string.Format(NextNumber, i + 1));
                        }
                        else
                        {
                            textEditor.Document.Insert(this.newLine.Offset, match.Value);
                        }
                    }

                    this.currentLine = null;
                }
            }
            else
            {
                this.processing = false;
            }
        }

        private class NextBulletLineTracker : ILineTracker
        {
            private readonly Action<ISegment, ISegment> callback;

            public NextBulletLineTracker(Action<ISegment, ISegment> callback)
            {
                this.callback = callback;
            }

            public void BeforeRemoveLine(DocumentLine line)
            {
            }

            public void ChangeComplete(DocumentChangeEventArgs e)
            {
            }

            public void LineInserted(DocumentLine insertionPos, DocumentLine newLine)
            {
                this.callback(insertionPos, newLine);
            }

            public void RebuildDocument()
            {
            }

            public void SetLineLength(DocumentLine line, int newTotalLength)
            {
            }
        }
    }
}
