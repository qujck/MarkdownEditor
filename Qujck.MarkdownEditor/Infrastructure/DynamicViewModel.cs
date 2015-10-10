﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Qujck.MarkdownEditor.Infrastructure
{
    public abstract class DynamicViewModel : System.Windows.DependencyObject, 
        IDynamicMetaObjectProvider, INotifyPropertyChanged
    {
        protected InternalDynamicViewModel child { get; private set; }

        public DynamicViewModel(params IDictionary<string, object>[] propertySets)
        {
            this.child = new InternalDynamicViewModel(this, propertySets);
        }

        public DynamicViewModel(params string[] properties)
        {
            this.child = new InternalDynamicViewModel(this, properties);
        }

        DynamicMetaObject IDynamicMetaObjectProvider.GetMetaObject(Expression parameter)
        {
            return new ForwardingMetaObject(
                parameter,
                BindingRestrictions.Empty,
                this,
                this.child,
                expr => Expression.Property(expr, "child"));
        }

        public event PropertyChangedEventHandler PropertyChanged
        {
            add { this.child.PropertyChanged += value; }
            remove { this.child.PropertyChanged -= value; }
        }

        public object this[string key]
        {
            get
            {
                return this.child[key];
            }
            set
            {
                this.child[key] = value;
            }
        }

        public void Update(params IDictionary<string, object>[] propertySets)
        {
            this.child.Update(propertySets);
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
            var canExecute = this[name] as Func<bool>;
            if (canExecute == null)
            {
                throw new ArgumentNullException();
            }

            return canExecute();
        }

        public void Execute(string name)
        {
            var execute = this[name] as Action;
            if (execute == null)
            {
                throw new ArgumentNullException();
            }

            execute();
        }

        protected class InternalDynamicViewModel : DynamicModel
        {
            private INotifyPropertyChanged parent;

            public InternalDynamicViewModel(
                INotifyPropertyChanged parent,
                params IDictionary<string, object>[] propertySets) : 
                base(propertySets)
            {
                this.parent = parent;
            }

            public InternalDynamicViewModel(
                INotifyPropertyChanged parent,
                params string[] properties) : 
                base(properties)
            {
                this.parent = parent;
            }

            protected override void OnPropertyChanged(string propertyName)
            {
                base.OnPropertyChanged(this.parent, propertyName);
            }
        }
    }
}
