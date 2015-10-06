using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Qujck.MarkdownEditor
{
    public sealed class DependencyResolverExtension : MarkupExtension
    {
        private readonly Type serviceType;

        public DependencyResolverExtension(string typeName)
        {
            this.serviceType = Type.GetType(typeName);
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return CompositionRoot.Instance.Resolve(serviceType);
        }
    }
}
