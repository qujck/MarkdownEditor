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
    public sealed class DocumentViewRenderedViewInterceptKeyDownBehaviour : Behavior<DocumentView>
    {
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
            this.AssociatedObject.RenderedView.PreviewKeyDown += RenderedView_PreviewKeyDown;
        }

        private void AssociatedObject_Unloaded(object sender, RoutedEventArgs e)
        {
            this.AssociatedObject.RenderedView.PreviewKeyDown -= RenderedView_PreviewKeyDown;
        }

        private void RenderedView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            if (this.KeyIsDefinedAsShortcutKey(e.Key))
            {
                var e1 = new KeyEventArgs(
                    e.KeyboardDevice, 
                    e.KeyboardDevice.ActiveSource, 
                    e.Timestamp, 
                    e.Key)
                {
                    RoutedEvent = Keyboard.KeyDownEvent,
                };

                this.AssociatedObject.RaiseEvent(e1);
            }
        }

        private bool KeyIsDefinedAsShortcutKey(Key key)
        {
            foreach(var binding in this.AssociatedObject.InputBindings)
            {
                if (binding is KeyBinding)
                {
                    var keyBinding = binding as KeyBinding;
                    if (keyBinding.Key == key)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
   }
}