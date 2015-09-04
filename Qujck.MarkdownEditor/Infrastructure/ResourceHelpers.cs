using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Qujck.MarkdownEditor.Infrastructure
{
    public static class ResourceHelpers
    {
        static Assembly assembly = Assembly.GetExecutingAssembly();

        public static StringBuilder AppendResource(this StringBuilder sb, string name)
        {
            sb.AppendLine(ReadResource("Qujck.MarkdownEditor." + name));
            return sb;
        }

        public static StringBuilder AppendManyResources(this StringBuilder sb, string name)
        {
            var resources =
                from resource in assembly.GetManifestResourceNames()
                where resource.StartsWith("Qujck.MarkdownEditor." + name)
                select ReadResource(resource);
            resources.ToList().ForEach(langFile => sb.AppendLine(langFile));
            return sb;
        }

        public static string ReadResource(string name)
        {
            var resource = assembly.GetManifestResourceStream(name);
            return new StreamReader(resource).ReadToEnd();
        }
    }
}
