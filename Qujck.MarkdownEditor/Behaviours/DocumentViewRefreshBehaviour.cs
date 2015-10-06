using System;
using System.Collections.Generic;
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
using ICSharpCode.AvalonEdit;
using Qujck.MarkdownEditor.Queries;
using Qujck.MarkdownEditor.Commands;
using Qujck.MarkdownEditor.Infrastructure;

namespace Qujck.MarkdownEditor.Behaviours
{
    public sealed class DocumentViewRefreshBehaviour : BehaviourWithResolver<DocumentView>
    {
        private readonly DispatcherTimer textChangedRefreshRenderedViewTimer;

        public DocumentViewRefreshBehaviour()
        {
            this.textChangedRefreshRenderedViewTimer = new DispatcherTimer { Interval = new TimeSpan(100) };
            this.textChangedRefreshRenderedViewTimer.Tick += RefreshTimer_Tick;
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
            this.AssociatedObject.TextEditor.TextChanged += TextEditor_TextChanged;
        }

        private void AssociatedObject_Unloaded(object sender, RoutedEventArgs e)
        {
            this.AssociatedObject.TextEditor.TextChanged -= TextEditor_TextChanged;
        }

        private void TextEditor_TextChanged(object sender, EventArgs e)
        {
            if (!this.textChangedRefreshRenderedViewTimer.IsEnabled)
            {
                this.textChangedRefreshRenderedViewTimer.Start();
            }
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            this.RefreshView();
        }

        private void RefreshView()
        {
            if (this.textChangedRefreshRenderedViewTimer.IsEnabled &&
                this.textChangedRefreshRenderedViewTimer.Tag == null)
            {
                this.textChangedRefreshRenderedViewTimer.Stop();
                this.textChangedRefreshRenderedViewTimer.Tag = this;

                var writeCommand = this.DependencyResolver.Resolve<ICommandHandler<Command.WriteDocument>>();

                writeCommand.Run(
                    (scriptName, args) => this.AssociatedObject.RenderedView.InvokeScript(scriptName, args),
                    this.AssociatedObject.TextEditor.Text);

                this.textChangedRefreshRenderedViewTimer.Tag = null;
            }
        }
    }
}