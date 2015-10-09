using System;
using System.Collections.Generic;
using System.Xaml;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Qujck.MarkdownEditor.Infrastructure
{
    public sealed partial class Container : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var provideValueTarget = (IProvideValueTarget)serviceProvider
                .GetService(typeof(IProvideValueTarget));
            var targetProperty = provideValueTarget.TargetProperty as PropertyInfo;

            if (targetProperty == null)
            {
                throw new InvalidProgramException();
            }

            return _instance.Resolve(targetProperty.PropertyType);
        }
    }
}
