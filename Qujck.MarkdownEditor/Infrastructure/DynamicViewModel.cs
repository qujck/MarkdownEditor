using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;

namespace Qujck.MarkdownEditor.Infrastructure
{
    public abstract class DynamicViewModel : DynamicObject, INotifyPropertyChanged
    {
        private IDictionary<string, object> dictionary { get; set; }

        protected DynamicViewModel(params IDictionary<string, object>[] propertySets)
        {
            this.dictionary = new Dictionary<string, object>();
            this.Update(propertySets);
        }

        protected DynamicViewModel(params string[] properties)
        {
            this.dictionary = new Dictionary<string, object>();
            foreach (var property in properties)
            {
                this.dictionary.Add(property, null);
            }
        }

        public int Count { get { return this.dictionary.Keys.Count; } }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (!this.dictionary.ContainsKey(binder.Name))
            {
                throw new InvalidProgramException(this.PropertyNotFoundException(binder.Name));
            }

            result = this.GetValue(binder.Name);

            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (!this.dictionary.ContainsKey(binder.Name))
            {
                throw new InvalidProgramException(this.PropertyNotFoundException(binder.Name));
            }

            this.SetValue(binder.Name, value);

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
            protected set
            {
                this.SetValue(key, value);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void Update(params IDictionary<string, object>[] propertySets)
        {
            foreach (var properties in propertySets)
            {
                foreach (var property in properties)
                {
                    this.SetValue(property.Key, property.Value);
                }
            }
        }

        protected void RegisterCommandAction(string name, Action action)
        {
            this[name] = action;
        }

        protected void RegisterCommandAction(string name, Action action, string canName, Func<bool> canAction)
        {
            this[canName] = canAction;
            this.RegisterCommandAction(name, action);
        }

        public bool CanExecute(string name)
        {
            var canExecute = this.dictionary[name] as Func<bool>;
            if (canExecute == null)
            {
                throw new ArgumentNullException();
            }

            return canExecute();
        }

        public void Execute(string name)
        {
            var execute = this.dictionary[name] as Action;
            if (execute == null)
            {
                throw new ArgumentException();
            }

            execute();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
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
