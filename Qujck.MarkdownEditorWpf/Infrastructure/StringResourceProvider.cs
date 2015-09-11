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
        public string Many(params string[] prefixes)
        {
            var resources =
                from resource in Assembly.GetExecutingAssembly().GetManifestResourceNames()
                from prefix in prefixes
                where resource.StartsWith("Qujck.MarkdownEditor." + prefix)
                select ReadResource(resource);
            var sb = new StringBuilder();
            resources.ToList().ForEach(langFile => sb.AppendLine(langFile));
            return sb.ToString();
        }

        public string Single(params string[] names)
        {
            return string.Join(Environment.NewLine,
                from name in names 
                select ReadResource("Qujck.MarkdownEditor." + name));
        }

        private static string ReadResource(string name)
        {
            var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
            return new StreamReader(resource).ReadToEnd();
        }
    }
}
