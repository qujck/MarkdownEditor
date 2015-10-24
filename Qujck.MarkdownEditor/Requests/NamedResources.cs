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
            this IStringRequestHandler<NamedResources> handler,
            params string[] names)
        {
            return handler.Execute(new NamedResources(names));
        }

        internal sealed class NamedResources : IStringRequest
        {
            internal NamedResources(params string[] names)
            {
                this.Names = names;
            }

            public string[] Names { get; set; }
        }

        internal static partial class Handlers
        {
            internal sealed class NamedResourcesHandler : IStringRequestHandler<NamedResources>
            {
                public string Execute(NamedResources query)
                {
                    return string.Join(Environment.NewLine,
                        from name in query.Names
                        select ReadResource("Qujck.MarkdownEditor." + name));
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
