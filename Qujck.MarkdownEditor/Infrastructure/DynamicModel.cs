using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;

namespace Qujck.MarkdownEditor.Infrastructure
{
    /// <summary>
    /// Provides a base class for specifying dynamic behavior at run time. This class
    /// must be inherited from; you cannot instantiate it directly.
    /// </summary>
    public abstract class DynamicModel : DynamicObject, INotifyPropertyChanged
    {
        private readonly IDictionary<string, object> dictionary;

        /// <summary>
        /// Enables derived types to initialize a new instance of the DynamicViewModel
        /// type with one or more Dictionaries of prepared property names and values.
        /// </summary>
        protected DynamicModel(params IDictionary<string, object>[] propertySets)
        {
            this.dictionary = new Dictionary<string, object>();
            this.Update(propertySets);
        }

        /// <summary>
        /// Enables derived types to initialize a new instance of the DynamicViewModel
        /// type with a list of predefined property names.
        /// </summary>
        protected DynamicModel(params string[] properties)
        {
            this.dictionary = new Dictionary<string, object>();
            foreach (var property in properties)
            {
                this[property] = null;
            }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (!this.dictionary.ContainsKey(binder.Name))
            {
                throw new InvalidProgramException(this.PropertyNotFoundException(binder.Name));
            }

            result = this[binder.Name];

            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (!this.dictionary.ContainsKey(binder.Name))
            {
                throw new InvalidProgramException(this.PropertyNotFoundException(binder.Name));
            }

            this[binder.Name] = value;

            return true;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            if (this.dictionary.ContainsKey(binder.Name) && this.dictionary[binder.Name] is Delegate)
            {
                Delegate del = this.dictionary[binder.Name] as Delegate;
                result = del.DynamicInvoke(args);
                return true;
            }

            return base.TryInvokeMember(binder, args, out result);
        }

        public override bool TryDeleteMember(DeleteMemberBinder binder)
        {
            if (this.dictionary.ContainsKey(binder.Name))
            {
                this.dictionary.Remove(binder.Name);
                return true;
            }

            return base.TryDeleteMember(binder);
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            foreach (string name in this.dictionary.Keys)
            {
                yield return name;
            }
        }

        public object this[string key]
        {
            get
            {
                return this.GetValue(key);
            }
            set
            {
                this.SetValue(key, value);
            }
        }

        public void Update(params IDictionary<string, object>[] propertySets)
        {
            foreach (var properties in propertySets)
            {
                foreach (var property in properties)
                {
                    this[property.Key] = property.Value;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected void OnPropertyChanged(object sender, string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(sender, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string PropertyNotFoundException(string name)
        {
            return string.Format("Property `{0}` not found.", name);
        }

        private object GetValue(string name)
        {
            return this.dictionary[name];
        }

        private void SetValue(string name, object value)
        {
            if (!this.dictionary.ContainsKey(name) ||
                !EqualityComparer<object>.Default.Equals(this.dictionary[name], value))
            {
                this.dictionary[name] = value;
                this.OnPropertyChanged(name);
            }
        }
    }
}
