using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Qujck.MarkdownEditor.Requests
{
    internal static partial class Strings
    {
        internal static string Execute(
            this IStringRequestHandler<PrefixedResources> handler,
            params string[] prefixes)
        {
            return handler.Execute(new PrefixedResources(prefixes));
        }

        internal sealed class PrefixedResources : IStringRequest
        {
            internal PrefixedResources(params string[] prefixes)
            {
                this.Prefixes = prefixes;
            }

            public string[] Prefixes { get; set; }
        }

        internal static partial class Handlers
        {
            internal sealed class PrefixedResourcesHandler : IStringRequestHandler<PrefixedResources>
            {
                public string Execute(PrefixedResources query)
                {
                    var resources =
                        from resource in Assembly.GetExecutingAssembly().GetManifestResourceNames()
                        from prefix in query.Prefixes
                        where resource.StartsWith("Qujck.MarkdownEditor." + prefix)
                        select ReadResource(resource);
                    var sb = new StringBuilder();
                    resources.ToList().ForEach(langFile => sb.AppendLine(langFile));
                    return sb.ToString();
                }

                private static string ReadResource(string name)
                {
                    var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
                    return new StreamReader(resource).ReadToEnd();
                }
            }
        }
    }
}
