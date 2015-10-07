using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qujck.MarkdownEditor
{
    public sealed class DocumentViewModel : AbstractDynamicObject
    {
        public DocumentViewModel() : base(
            new KeyValuePair<string, object>("LeftColumnWidth", "1*"),
            new KeyValuePair<string, object>("RightColumnWidth", "1*"),
            new KeyValuePair<string, object>("TopRowHeight", "1*"),
            new KeyValuePair<string, object>("BottomRowHeight", "0"),
            new KeyValuePair<string, object>("TextEditorColumn", "0"),
            new KeyValuePair<string, object>("TextEditorRow", "0"),
            new KeyValuePair<string, object>("RenderedViewColumn", "1"),
            new KeyValuePair<string, object>("RenderedViewRow", "0"))
        {
        }
    }
}