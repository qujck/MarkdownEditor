using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Qujck.MarkdownEditor.Infrastructure
{
    public sealed class StringResourceProvider : IStringResourceProvider
    {
        public string Many(string name)
        {
            var resources =
                from resource in Assembly.GetExecutingAssembly().GetManifestResourceNames()
                where resource.StartsWith("Qujck.MarkdownEditor." + name)
                select ReadResource(resource);
            var sb = new StringBuilder();
            resources.ToList().ForEach(langFile => sb.AppendLine(langFile));
            return sb.ToString();
        }

        public string Single(string name)
        {
            return ReadResource("Qujck.MarkdownEditor." + name);
        }

        private static string ReadResource(string name)
        {
            var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
            return new StreamReader(resource).ReadToEnd();
        }
    }
}
