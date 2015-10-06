using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;

namespace Qujck.MarkdownEditor.Behaviours
{
    public abstract class BehaviourWithResolver<T> : Behavior<T> where T : DependencyObject
    {
        public static readonly DependencyProperty DependencyResolverProperty =
            DependencyProperty.Register(
                "DependencyResolver",
                typeof(IDependencyResolver),
                typeof(T));

        public IDependencyResolver DependencyResolver
        {
            get
            {
                var resolver = this.GetValue(DependencyResolverProperty) as IDependencyResolver;
                if (resolver == null)
                {
                    throw new InvalidProgramException();
                }
                return resolver;
            }
            set
            {
                this.SetValue(DependencyResolverProperty, value);
            }
        }
    }
}
