using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qujck.MarkdownEditor.Infrastructure
{
    public static class Constants
    {
        public struct Content
        {
            private const string Prefix = "Content.";
            public const string Layout = Prefix + "layout.html";
            public const string Bootstrap = Prefix + "bootstrap.min.css";
            public const string SiteCss = Prefix + "site.css";
        }

        public struct Scripts
        {
            private const string Prefix = "Scripts.";
            public const string Marked = Prefix + "marked.min.js";
        }
    }
}
