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
using ICSharpCode.AvalonEdit.Highlighting;
using Qujck.MarkdownEditor.Requests;
using Qujck.MarkdownEditor.Actions;
using Qujck.MarkdownEditor.Infrastructure;

namespace Qujck.MarkdownEditor.Behaviours
{
    internal sealed class AvalonEditInitialiser : Behavior<DocumentView>
    {
        public IStringRequestHandler<Strings.NamedResources> NamedResources { private get; set; }

        protected override void OnAttached()
        {
            base.OnAttached();
            base.AssociatedObject.Loaded += AssociatedObject_Loaded;
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            base.AssociatedObject.Loaded -= AssociatedObject_Loaded;
            this.LoadHighlighter();
            this.AssociatedObject.TextEditor.Options.EnableHyperlinks = false;
            this.AssociatedObject.TextEditor.Options.EnableEmailHyperlinks = false;
            this.AssociatedObject.TextEditor.Options.ConvertTabsToSpaces = true;
        }

        public void LoadHighlighter()
        {
            // Load our custom highlighting definition
            IHighlightingDefinition customHighlighting;
            string resource = this.NamedResources.Execute("Content.Markdown2.xshd");
            using (XmlReader reader = XmlReader.Create(new StringReader(resource)))
            {
                customHighlighting = ICSharpCode.AvalonEdit.Highlighting.Xshd.
                    HighlightingLoader.Load(reader, HighlightingManager.Instance);

                HighlightingManager.Instance.RegisterHighlighting("MarkDown2", new string[] { ".md" }, customHighlighting);

                this.AssociatedObject.TextEditor.SyntaxHighlighting = customHighlighting;
            }
        }
    }
}
