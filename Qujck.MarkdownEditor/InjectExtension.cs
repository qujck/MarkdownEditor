using System;
using System.Collections.Generic;
using System.Xaml;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Qujck.MarkdownEditor
{
    public sealed partial class InjectExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var target = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            var targetProperty = target.TargetProperty as PropertyInfo;

            if (targetProperty == null)
            {
                throw new InvalidProgramException();
            }

            return CompositionRoot.Instance.Resolve(targetProperty.PropertyType);
        }
    }
}
