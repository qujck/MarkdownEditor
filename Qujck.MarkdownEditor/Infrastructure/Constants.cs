using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qujck.MarkdownEditor
{
    public static partial class Constants
    {
        public struct Content
        {
            private const string Prefix = "Content.";
            public const string Layout = Prefix + "layout.html";
            public const string Bootstrap = Prefix + "bootstrap.min.css";
            public const string SiteCss = Prefix + "site.css";
            public const string StackOverflowCss = Prefix + "stackoverflow.css";

            public struct Scripts
            {
                private const string Prefix = Content.Prefix + "Scripts.";
                public const string Marked = Prefix + "marked.min.js";
                public const string Prettify = Prefix + "Prettify.prettify.js";
                public const string PrettifyLang = Prefix + "Prettify.lang-";
                public const string EmbeddedPrettifyAction =
@"function prettifyCodeSamples() {
    var text = document.getElementById('content').innerHTML;
    var result = text.replace(/<pre>/gi, '<pre class=""prettyprint"">');
    document.getElementById('content').innerHTML = result;
    prettyPrint();
}";
                public const string EmbeddedPrettifyActionName = "prettifyCodeSamples";
            }
        }
    }
}
