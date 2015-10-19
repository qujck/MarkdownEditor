using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
        const string Bullet = "- ";
        private readonly NextBulletLineTracker tracker;
        private ISegment currentLine;
        private ISegment newLine;
        private bool insertingBullet;
        private bool removingBullet;

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

        private void LineInsertedCallBack(ISegment currentLine, ISegment newLine)
        {
            this.currentLine = currentLine;
            this.newLine = newLine;
        }

        private void TextEditor_TextChanged(object sender, EventArgs e)
        {
            if (!this.insertingBullet && 
                !this.removingBullet &&
                this.currentLine != null)
            {
                string text = this.AssociatedObject.TextEditor.Document.GetText(this.currentLine);
                if (text == Bullet)
                {
                    this.removingBullet = true;
                    this.AssociatedObject.TextEditor.Document.Remove(this.currentLine);
                }
                else if (text.StartsWith(Bullet))
                {
                    this.insertingBullet = true;
                    this.AssociatedObject.TextEditor.Document.Insert(this.newLine.Offset, Bullet);
                }
                this.currentLine = null;
            }
            else
            {
                this.insertingBullet = false;
                this.removingBullet = false;
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
